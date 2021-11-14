using EventFlow.EntityFramework;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using rover.infrastructure.ef;
using System.Data.Common;

namespace rover.unittests.Utilities
{
    internal class FakedEntityFramewokReadModelDbContextProvider : IDbContextProvider<DBContextControlRoom>
    {
        private static DBContextControlRoom _dbContextControlRoom;
        public DBContextControlRoom CreateContext()
        {
            var options = new DbContextOptionsBuilder<DBContextControlRoom>()
                //.UseSqlite(CreateInMemoryDatabase())
                .UseInMemoryDatabase("for testing")
                .Options;

            var context = new DBContextControlRoom(options);
            context.Database.EnsureCreated();
            _dbContextControlRoom = context;
            return context;
        }

        private static DbConnection CreateInMemoryDatabase()
        {
            var connection = new SqliteConnection("Filename=:memory:");

            connection.Open();

            return connection;
        }
    }
}
