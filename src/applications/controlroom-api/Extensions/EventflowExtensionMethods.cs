namespace controlroom_api.Extensions;

static class EventflowExtensionMethods
{
    public static IServiceCollection AddEventFlow(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEventFlow(ef =>
        {
            ef.UseServiceCollection(services)
                .AddAspNetCoreMetadataProviders()
                //.AddAspNetCore(o => o.AddDefaultMetadataProviders())
                .AddEvents(typeof(StartedEvent))
                .AddCommands(typeof(StartCommand))
                .AddCommandHandlers(typeof(StartCommandHandler))

                .AddEvents(typeof(StoppedEvent))
                .AddCommands(typeof(ChangePositionCommand))
                .AddCommandHandlers(typeof(ChangePositionCommandHandler))

                .AddEvents(typeof(MovedEvent))
                .AddEvents(typeof(LandedEvent))

                .RegisterServices(sr =>
                {
                    sr.Register<IPositionRepository, PositionRepository>(Lifetime.Scoped);
                    sr.Register<IDbContextProvider<DBContextControlRoom>, DBContextProvider>();
                })
                .AddEntityFrameworkReadModel()

                .AddQueryHandler<GetPositionsQueryHandler, GetPositionsQuery, List<PositionReadModel>>()
                .AddQueryHandler<GetLastPositionQueryHandler, GetLastPositionQuery, PositionReadModel>()
                .AddQueryHandler<GetLandingPositionQueryHandler, GetLandingPositionQuery, LandingReadModel>()

                //.UseMssqlReadModel<PositionReadModel>()
                //.UseMssqlReadModel<StartReadModel>()
                //.ConfigureMsSql(MsSqlConfiguration.New.SetConnectionString(
                //    Configuration.GetConnectionString("ReadModelsConnection"))

                .UseHangfireJobScheduler()
                .AddJobs(new[] { typeof(SendMessageToRoverJob), typeof(HandlingRoverMessagesJob) })

                .PublishToRabbitMq(RabbitMqConfiguration.With(
                                new Uri(configuration.GetSection(nameof(IntegrationSettings)).GetValue<string>("RabbitMQConnectionString")),
                                true, 5,
                                configuration.GetSection(nameof(IntegrationSettings)).GetValue<string>("RabbitMQPublishExchange")))

                .AddAsynchronousSubscriber<RoverAggregate, RoverAggregateId, MovedEvent, StoppedEventSubscriber>()
                .RegisterServices(s =>
                {
                    s.Register<IHostedService, RabbitConsumePersistenceService>(Lifetime.Singleton);
                    s.Register<IHostedService, StoppedEventSubscriber>(Lifetime.Singleton);

                })
                .UseConsoleLog();
        });

        return services;
    }
}