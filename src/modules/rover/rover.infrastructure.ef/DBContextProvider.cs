using EventFlow.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace rover.infrastructure.ef
{
    public class DBContextProvider : IDbContextProvider<DBContextControlRoom>, IDisposable
    {
        private readonly DbContextOptions<DBContextControlRoom> _options;

        public DBContextProvider(string msSqlConnectionString)
        {
            _options = new DbContextOptionsBuilder<DBContextControlRoom>()
                .UseSqlServer(msSqlConnectionString)
                .Options;
        }

        public DBContextControlRoom CreateContext()
        {
            var context = new DBContextControlRoom(_options);
            context.Database.EnsureCreated();
            return context;
        }

        public void Dispose()
        {
        }
    }
}
