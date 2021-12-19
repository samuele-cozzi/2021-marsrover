namespace rover.integration.tests;

internal class TestHelpers
{
    //internal IResolver Resolver_LandingLat0Long0FacE_QueryEF(string? connectionString = null)
    //{
    //    var services = new ServiceCollection();
    //    // Settings
    //    this.CreateRoverSettings_LandingLat0Long0FacE(services);
    //    this.CreateMarsSettings_Step1_ObstacleLat0Long2(services);
    //    this.CreateIntegrationSettings_0(services);
    //    this.CreateConnectionString(services, connectionString);

    //    // EventFlow
    //    var resolver = EventFlowOptions.New
    //        .UseServiceCollection(services)
    //        .AddAspNetCore()

    //        .UseEntityFrameworkReadModel<PositionReadModel, DBContextControlRoom>()
    //        .ConfigureEntityFramework(EntityFrameworkConfiguration.New)
    //        .AddDbContextProvider<DBContextControlRoom, DBContextProvider>()

    //        .AddQueryHandler<GetLastPositionQueryHandler, GetLastPositionQuery, PositionReadModel>()
    //        .AddQueryHandler<GetLandingPositionQueryHandler, GetLandingPositionQuery, LandingReadModel>()

    //        .UseEntityFrameworkReadModel<StartReadModel, DBContextControlRoom>()
    //        .UseEntityFrameworkReadModel<PositionReadModel, DBContextControlRoom>()
    //        .UseEntityFrameworkReadModel<LandingReadModel, DBContextControlRoom>()

    //        .RegisterServices(s => {
    //            s.Register<IPositionRepository, rover.infrastructure.ef.PositionRepository>();
    //            s.Register<IDbContextProvider<DBContextControlRoom>, DBContextProvider>(Lifetime.AlwaysUnique);
    //            s.CreateResolver(true);
    //        })
    //        .CreateResolver();

    //    return resolver;
    //}

    internal IResolver Resolver_LandingLat0Long0FacE_QueryDapper(string? connectionString = null)
    {
        var services = new ServiceCollection();
        // Settings
        this.CreateRoverSettings_LandingLat0Long0FacE(services);
        this.CreateMarsSettings_Step1_ObstacleLat0Long2(services);
        this.CreateIntegrationSettings_0(services);
        this.CreateConnectionString(services, connectionString);

        // EventFlow
        var resolver = EventFlowOptions.New
            .UseServiceCollection(services)
            .AddAspNetCore()

            .UseEntityFrameworkReadModel<PositionReadModel, DBContextControlRoom>()
            .ConfigureEntityFramework(EntityFrameworkConfiguration.New)
            .AddDbContextProvider<DBContextControlRoom, DBContextProvider>()

            .AddQueryHandler<GetLastPositionQueryHandler, GetLastPositionQuery, PositionReadModel>()
            .AddQueryHandler<GetLandingPositionQueryHandler, GetLandingPositionQuery, LandingReadModel>()

            .UseInMemoryReadStoreFor<StartReadModel>()
            .UseInMemoryReadStoreFor<PositionReadModel>()
            .UseInMemoryReadStoreFor<LandingReadModel>()

            .RegisterServices(s => {
                s.Register<IPositionRepository, rover.infrastructure.dapper.PositionRepository>();
                s.Register<IDbContextProvider<DBContextControlRoom>, DBContextProvider>(Lifetime.AlwaysUnique);
                s.Register<IDbConnectionFactory, DapperConnectionFactory>(Lifetime.AlwaysUnique);
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

    private ServiceCollection CreateConnectionString(ServiceCollection services, string? connectionString = null)
    {
        var myConfiguration = new Dictionary<string, string>
            {
                { "ConnectionStrings:ReadModelsConnection", 
                    (string.IsNullOrWhiteSpace(connectionString))
                    ? "Server=localhost,5433;Database=RoverRM;User Id=sa;Password=Pass@word"
                    : connectionString 
                }
            };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(myConfiguration)
            .Build();

        services.AddSingleton<IConfiguration>(configuration);

        return services;
    }

    #endregion
}
