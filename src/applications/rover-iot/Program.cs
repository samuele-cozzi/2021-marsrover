// See https://aka.ms/new-console-template for more information

IHost host = Host.CreateDefaultBuilder(args)
.ConfigureServices((hostcontext, services) => {
    var configurationRoot = hostcontext.Configuration;
    services.AddOptions();
    services.Configure<RoverSettings>(configurationRoot.GetSection(nameof(RoverSettings)));
    services.Configure<IntegrationSettings>(configurationRoot.GetSection(nameof(IntegrationSettings)));
    services.Configure<MarsSettings>(configurationRoot.GetSection(nameof(MarsSettings)));
    services.AddScoped<InitMarsService>();

    EventFlowOptions.New
        .Configure(cfg => cfg.IsAsynchronousSubscribersEnabled = true)
        .UseServiceCollection(services)
        .AddAspNetCoreMetadataProviders()
        //.AddAspNetCore(o => o.AddDefaultMetadataProviders())
        .PublishToRabbitMq(RabbitMqConfiguration.With(
            new Uri(configurationRoot.GetSection(nameof(IntegrationSettings)).GetValue<string>("RabbitMQConnectionString")),
            true, 5,
            configurationRoot.GetSection(nameof(IntegrationSettings)).GetValue<string>("RabbitMQPublishExchange")))

        //.AddEvents(typeof(StartedEvent))
        //.AddCommands(typeof(StartCommand))
        //.AddCommandHandlers(typeof(StartCommandHandler))

        .AddEvents(typeof(LandedEvent))
        .AddEvents(typeof(StartedEvent))
        .AddEvents(typeof(MovedEvent))
        .AddEvents(typeof(StoppedEvent))

        .AddCommands(typeof(MoveCommand))
        .AddCommandHandlers(typeof(MoveCommandHandler))

        //
        // subscribe services changed
        //
        .AddAsynchronousSubscriber<RoverAggregate, RoverAggregateId, StartedEvent, StartEventSubscriber>()
        .RegisterServices(s => {
            s.Register<IHostedService, RabbitConsumePersistenceService>(Lifetime.Singleton);
            s.Register<IHostedService, StartEventSubscriber>(Lifetime.Singleton);
        });
})
.Build();

await host.Services.GetService<InitMarsService>()?.InitMarsAsync(CancellationToken.None);
await host.RunAsync();