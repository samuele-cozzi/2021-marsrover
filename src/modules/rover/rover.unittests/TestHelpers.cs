using EventFlow;
using EventFlow.AspNetCore.Extensions;
using EventFlow.Configuration;
using EventFlow.DependencyInjection.Extensions;
using EventFlow.EntityFramework;
using EventFlow.EntityFramework.Extensions;
using EventFlow.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using rover.domain.Aggregates;
using rover.domain.Commands;
using rover.domain.DomainEvents;
using rover.domain.Jobs;
using rover.domain.Models;
using rover.domain.Queries;
using rover.domain.Services;
using rover.domain.Settings;
using rover.infrastructure.ef;
using rover.unittests.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rover.unittests
{
    internal class TestHelpers
    {
        internal IResolver Resolver_LandingLat0Long0FacE_Step1_ObstacleLat0Long2()
        {
            var services = new ServiceCollection();
            // Settings
            this.CreateRoverSettings_LandingLat0Long0FacE(services);
            this.CreateMarsSettings_Step1_ObstacleLat0Long2(services);
            this.CreateIntegrationSettings_0(services);

            // EventFlow
            var provider = EventFlowOptions.New
                .UseServiceCollection(services)
                .AddAspNetCore()

                .UseEntityFrameworkReadModel<PositionReadModel, DBContextControlRoom>()
                .ConfigureEntityFramework(EntityFrameworkConfiguration.New)
                .AddDbContextProvider<DBContextControlRoom, FakedEntityFramewokReadModelDbContextProvider>()

                .AddEvents(typeof(StartedEvent))
                .AddEvents(typeof(MovedEvent))
                .AddEvents(typeof(StoppedEvent))
                .AddEvents(typeof(LandedEvent))

                .AddJobs(typeof(SendMessageToRoverJob))
                .AddJobs(typeof(HandlingRoverMessagesJob))
                
                .AddQueryHandler<GetLastPositionQueryHandler, GetLastPositionQuery, PositionReadModel>()
                .AddQueryHandler<GetLandingPositionQueryHandler, GetLandingPositionQuery, LandingReadModel>()

                .UseInMemoryReadStoreFor<StartReadModel>()
                .UseInMemoryReadStoreFor<PositionReadModel>()
                .UseInMemoryReadStoreFor<LandingReadModel>()
                
                .RegisterServices(s => {
                    s.Register<IPositionRepository, PositionRepository>();
                    s.Register<IDbContextProvider<DBContextControlRoom>, FakedEntityFramewokReadModelDbContextProvider>(Lifetime.AlwaysUnique);
                    s.CreateResolver(true);
                })
                .CreateServiceProvider();

            var resolver = provider.GetService<IResolver>();

            return resolver;
        }

        internal IResolver Resolver_LandingLat0Long0FacE_QueryEF()
        {
            var services = new ServiceCollection();
            // Settings
            this.CreateRoverSettings_LandingLat0Long0FacE(services);
            this.CreateMarsSettings_Step1_ObstacleLat0Long2(services);
            this.CreateIntegrationSettings_0(services);

            // EventFlow
            var provider = EventFlowOptions.New
                .UseServiceCollection(services)
                .AddAspNetCore()

                .UseEntityFrameworkReadModel<PositionReadModel, DBContextControlRoom>()
                .ConfigureEntityFramework(EntityFrameworkConfiguration.New)
                .AddDbContextProvider<DBContextControlRoom, FakedEntityFramewokReadModelDbContextProvider>()
                
                .AddQueryHandler<GetLastPositionQueryHandler, GetLastPositionQuery, PositionReadModel>()
                .AddQueryHandler<GetLandingPositionQueryHandler, GetLandingPositionQuery, LandingReadModel>()

                .UseInMemoryReadStoreFor<StartReadModel>()
                .UseInMemoryReadStoreFor<PositionReadModel>()
                .UseInMemoryReadStoreFor<LandingReadModel>()
                
                .RegisterServices(s => {
                    s.Register<IPositionRepository, rover.infrastructure.ef.PositionRepository>();
                    s.Register<IDbContextProvider<DBContextControlRoom>, FakedEntityFramewokReadModelDbContextProvider>(Lifetime.AlwaysUnique);
                    s.CreateResolver(true);
                })
                .CreateServiceProvider();

            var resolver = provider.GetService<IResolver>();

            return resolver;
        }

        internal IResolver Resolver_LandingLat0Long0FacE_QueryDapper()
        {
            var services = new ServiceCollection();
            // Settings
            this.CreateRoverSettings_LandingLat0Long0FacE(services);
            this.CreateMarsSettings_Step1_ObstacleLat0Long2(services);
            this.CreateIntegrationSettings_0(services);

            // EventFlow
            var provider = EventFlowOptions.New
                .UseServiceCollection(services)
                .AddAspNetCore()

                .UseEntityFrameworkReadModel<PositionReadModel, DBContextControlRoom>()
                .ConfigureEntityFramework(EntityFrameworkConfiguration.New)
                .AddDbContextProvider<DBContextControlRoom, FakedEntityFramewokReadModelDbContextProvider>()
                
                .AddQueryHandler<GetLastPositionQueryHandler, GetLastPositionQuery, PositionReadModel>()
                .AddQueryHandler<GetLandingPositionQueryHandler, GetLandingPositionQuery, LandingReadModel>()

                .UseInMemoryReadStoreFor<StartReadModel>()
                .UseInMemoryReadStoreFor<PositionReadModel>()
                .UseInMemoryReadStoreFor<LandingReadModel>()
                
                .RegisterServices(s => {
                    s.Register<IPositionRepository, rover.infrastructure.dapper.PositionRepository>();
                    s.Register<IDbContextProvider<DBContextControlRoom>, FakedEntityFramewokReadModelDbContextProvider>(Lifetime.AlwaysUnique);
                    s.CreateResolver(true);
                })
                .CreateServiceProvider();

            var resolver = provider.GetService<IResolver>();

            return resolver;
        }

        internal IRootResolver Resolver_Commands_LandingLat0Long0FacE_Step1_ObstacleLat0Long2()
        {
            var services = new ServiceCollection();
            // Settings
            this.CreateRoverSettings_LandingLat0Long0FacE(services);
            this.CreateMarsSettings_Step1_ObstacleLat0Long2(services);
            this.CreateIntegrationSettings_0(services);

            // EventFlow
            var resolver = EventFlowOptions.New
                .UseServiceCollection(services)
                .AddAspNetCore()

                .UseEntityFrameworkReadModel<PositionReadModel, DBContextControlRoom>()
                .ConfigureEntityFramework(EntityFrameworkConfiguration.New)
                .AddDbContextProvider<DBContextControlRoom, FakedEntityFramewokReadModelDbContextProvider>()

                .AddCommands(typeof(MoveCommand))
                .AddCommandHandlers(typeof(MoveCommandHandler))
                .AddCommands(typeof(ChangePositionCommand))
                .AddCommandHandlers(typeof(ChangePositionCommandHandler))
                .AddCommands(typeof(StartCommand))
                .AddCommandHandlers(typeof(StartCommandHandler))

                .AddEvents(typeof(StartedEvent))
                .AddEvents(typeof(MovedEvent))
                .AddEvents(typeof(StoppedEvent))
                .AddEvents(typeof(LandedEvent))

                .AddQueryHandler<GetLastPositionQueryHandler, GetLastPositionQuery, PositionReadModel>()
                .AddQueryHandler<GetLandingPositionQueryHandler, GetLandingPositionQuery, LandingReadModel>()

                .UseInMemoryReadStoreFor<StartReadModel>()
                .UseInMemoryReadStoreFor<PositionReadModel>()
                .UseInMemoryReadStoreFor<LandingReadModel>()

                .AddJobs(typeof(SendMessageToRoverJob))
                .AddJobs(typeof(HandlingRoverMessagesJob))

                .RegisterServices(s => {
                    s.Register<IPositionRepository, PositionRepository>();
                    s.Register<IDbContextProvider<DBContextControlRoom>, FakedEntityFramewokReadModelDbContextProvider>(Lifetime.AlwaysUnique);
                    s.CreateResolver(true);
                })
                .CreateResolver();

            return resolver;
        }



        #region settings

        private ServiceCollection CreateRoverSettings_LandingLat0Long0FacE(ServiceCollection services)
        {
            services.AddTransient<IOptions<RoverSettings>>(
                provider => Options.Create<RoverSettings>(new RoverSettings
                {
                    Landing = new Position()
                    {
                        FacingDirection = FacingDirections.E,
                        Coordinate = new Coordinate()
                        {
                            AngularPrecision = 0.5,
                            Latitude = 0,
                            Longitude = 0
                        }
                    }
                }));
            services.AddTransient<RoverSettings>();

            return services;
        }

        private ServiceCollection CreateMarsSettings_Step1_ObstacleLat0Long2(ServiceCollection services)
        {
            services.AddTransient<IOptions<MarsSettings>>(
                provider => Options.Create<MarsSettings>(new MarsSettings
                {
                    AngularPartition = 360,
                    Obstacles = new List<Coordinate>()
                    {
                        new Coordinate()
                        {
                            Latitude = 0,
                            Longitude = 2
                        }
                    }
                }));
            services.AddTransient<MarsSettings>();

            return services;
        }

        private ServiceCollection CreateIntegrationSettings_0(ServiceCollection services)
        {
            services.AddTransient<IOptions<IntegrationSettings>>(
                provider => Options.Create<IntegrationSettings>(new IntegrationSettings
                {
                    TimeDistanceOfMessageInSeconds = 0,
                    TimeDistanceOfVoyageInSeconds = 0
                }));
            services.AddTransient<IntegrationSettings>();

            return services;
        }

        #endregion
    }
}
