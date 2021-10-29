using Microsoft.EntityFrameworkCore;
using EventFlow.EntityFramework.Extensions;
using rover.domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rover.infrastructure.ef
{
    public class DBContextControlRoom: DbContext
    {
        public DBContextControlRoom(DbContextOptions<DBContextControlRoom> options) : base(options)
        {
        }

        public DbSet<StartReadModel> Commands { get; set; }
        public DbSet<PositionReadModel> Positions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .AddEventFlowEvents()
                .AddEventFlowSnapshots();
        }
    }
}
