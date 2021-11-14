using EventFlow.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace rover.infrastructure.ef
{
    public class DBContextProvider : IDbContextProvider<DBContextControlRoom>
    {
        private readonly DbContextOptions<DBContextControlRoom> _options;

        public DBContextProvider(IConfiguration configuration)
        {
            _options = new DbContextOptionsBuilder<DBContextControlRoom>()
                .UseSqlServer(configuration.GetConnectionString("ReadModelsConnection"))
                .Options;
        }

        public DBContextControlRoom CreateContext()
        {
            var context = new DBContextControlRoom(_options);
            context.Database.EnsureCreated();
            return context;
        }
    }
}
