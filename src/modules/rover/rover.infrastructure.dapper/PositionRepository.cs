using Dapper;
using Microsoft.Extensions.Configuration;
using rover.domain.Models;
using rover.domain.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace rover.infrastructure.dapper
{
    public class PositionRepository : IPositionRepository
    {
        private readonly string _connectionString;
        public PositionRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ReadModelsConnection");
        }
        public async Task<LandingReadModel> GetLandingPositionsAsync(CancellationToken cancellationToken)
        {
            var sql = "select * from dbo.Landing order by timestamp";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var result = await connection.QueryFirstOrDefaultAsync<LandingReadModel>(sql);
                return result;
            }
        }

        public async Task<PositionReadModel> GetLastPositionsAsync(CancellationToken cancellationToken)
        {
            var sql = "select * from dbo.Positions order by timestamp";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var result = await connection.QueryFirstOrDefaultAsync<PositionReadModel>(sql);
                return result;
            }
        }

        public async Task<List<PositionReadModel>> GetPositionsAsync(CancellationToken cancellationToken)
        {
            var sql = @"select  
                    AggregateId,
                    CAST(JSON_VALUE(metadata, '$.timestamp') AS DATETIME2)  AS Timestamp,
                    JSON_VALUE(metadata, '$.aggregate_sequence_number') AS SequenceNumber,
                    JSON_VALUE(Data, '$.FacingDirection') AS FacingDirection,
                    JSON_VALUE(Data, '$.Latitude') AS Latitude,
                    JSON_VALUE(Data, '$.Longitude') AS Longitude,
                    JSON_VALUE(Data, '$.IsBlocked') AS IsBlocked
                from EventFlow
                WHERE JSON_VALUE(metadata, '$.event_name') = 'positionchanged'
                ORDER BY CAST(JSON_VALUE(metadata, '$.aggregate_sequence_number') AS INT) DESC";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var result = (await connection.QueryAsync<PositionReadModel>(sql)).ToList();
                
                return result;
            }
        }
    }
}
