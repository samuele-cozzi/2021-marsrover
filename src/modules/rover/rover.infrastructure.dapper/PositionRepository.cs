using Dapper;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Configuration;
using rover.domain.Models;
using rover.domain.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace rover.infrastructure.dapper
{
    public class PositionRepository : IPositionRepository
    {
        private readonly IDbConnectionFactory _factory;
        public PositionRepository(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<LandingReadModel> GetLandingPositionsAsync(CancellationToken cancellationToken)
        {
            using (var connection = _factory.CreateDbConnection())
            {
                var sql = @"select * from dbo.Landing 
                    order by sequencenumber desc, timestamp desc";
                var result = await connection.QueryFirstOrDefaultAsync<LandingReadModel>(sql);
                return result;
            }
        }

        public async Task<PositionReadModel> GetLastPositionsAsync(CancellationToken cancellationToken)
        {
            using (var connection = _factory.CreateDbConnection())
            {
                var sql = @"select * from dbo.Positions
                    order by timestamp desc";

                var result = await connection.QueryFirstOrDefaultAsync<PositionReadModel>(sql);
                return result;
            }
        }

        public async Task<List<PositionReadModel>> GetPositionsAsync(CancellationToken cancellationToken)
        {
            using (var connection = _factory.CreateDbConnection())
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


                var result = (await connection.QueryAsync<PositionReadModel>(sql)).ToList();

                return result;
            }
        }
    }
}
