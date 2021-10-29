using rover.domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rover.infrastructure.ef
{
    public static class DbInitializer
    {
        public static void Initialize(DBContextControlRoom context)
        {
            context.Database.EnsureCreated();

            //// Look for any students.
            //if (context.Positions.Any())
            //{
            //    return;   // DB has been seeded
            //}

            //var positions = new PositionReadModel[]
            //{
            //    new PositionReadModel{StartId=""},
            //};
            //context.SaveChanges();
        }
    }
}
