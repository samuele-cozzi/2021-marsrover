using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Configuration;
using rover.domain.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace rover.infrastructure.dapper
{
    public class DapperConnectionFactory : IDbConnectionFactory
    {
        private readonly IConfiguration _configuration;
        public DapperConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IDbConnection CreateDbConnection()
        {
            var connection = new SqlConnection(_configuration.GetConnectionString("ReadModelsConnection"));
            connection.Open();
            return connection;
        }
    }
}
