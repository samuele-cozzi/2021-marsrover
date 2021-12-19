using EventFlow.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using rover.infrastructure.ef;
using System.Data.Common;

namespace rover.unittests.Utilities
{
    internal class FakedEntityFramewokReadModelDbContextProvider : IDbContextProvider<DBContextControlRoom>
    {
        private readonly string _connectionString;
        public FakedEntityFramewokReadModelDbContextProvider(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ReadModelsConnection");
        }

        public DBContextControlRoom CreateContext()
        {
            var options = new DbContextOptionsBuilder<DBContextControlRoom>()
                .UseSqlite(CreateInMemoryDatabase())
                //.UseInMemoryDatabase(_connectionString) //Guid.NewGuid().ToString())
                .Options;

            var context = new DBContextControlRoom(options);
            context.Database.EnsureCreated();
            return context;
        }

        private DbConnection CreateInMemoryDatabase()
        {
            var connection = new Microsoft.Data.Sqlite.SqliteConnection($"Data Source={_connectionString};Mode=Memory;Cache=Shared");
            connection.Open();
            return connection;
        }
    }
}
