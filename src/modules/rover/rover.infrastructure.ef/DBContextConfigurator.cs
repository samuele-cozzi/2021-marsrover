using EventFlow;
using EventFlow.EntityFramework;
using EventFlow.EntityFramework.Extensions;
using rover.domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace rover.infrastructure.ef
{
    public static class DBContextConfigurator
    {
        public static IEventFlowOptions AddEntityFrameworkReadModel(this IEventFlowOptions efo)
        {
            return efo
                .UseEntityFrameworkReadModel<StartReadModel, DBContextControlRoom>()
                .UseEntityFrameworkReadModel<PositionReadModel, DBContextControlRoom>()
                .ConfigureEntityFramework(EntityFrameworkConfiguration.New)
                .AddDbContextProvider<DBContextControlRoom, DBContextProvider>();
        }
    }
}
