using EventFlow;
using EventFlow.Aggregates;
using EventFlow.AspNetCore.Extensions;
using EventFlow.Configuration;
using EventFlow.DependencyInjection.Extensions;
using EventFlow.Extensions;
using EventFlow.RabbitMQ;
using EventFlow.RabbitMQ.Extensions;
using EventFlow.Subscribers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using rover.application.Aggregates;
using rover.application.Commands;
using rover.application.DomainEvents;
using rover.application.Entities;
using rover.application.Models;
using rover.domain.Settings;
using rover.infrastructure.rabbitmq;
using Serilog;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace rover
{
    internal class Program
    {
        //static void Main(string[] args)
        //{
        //    // Setup Host
        //    var host = CreateHostBuilder(args).Build();

        //    //// Invoke Worker
        //    //using IServiceScope serviceScope = host.Services.CreateScope();
        //    //IServiceProvider provider = serviceScope.ServiceProvider;

        //    //Console.WriteLine("Hello World!");

        //    host.Run();
        //}

        //private static IHostBuilder CreateHostBuilder(string[] args)
        //{
        //    return Host.CreateDefaultBuilder(args)
        //        //.UseServiceProviderFactory(new AutofacServiceProviderFactory())
        //        .UseSerilog((host, loggerConfiguration) =>
        //        {
        //            loggerConfiguration.ReadFrom.Configuration(host.Configuration);
        //        })
        //        //.UseServiceProviderFactory(new AutofacServiceProviderFactory())
        //        .ConfigureAppConfiguration((host, config) =>
        //        {
        //            config.Sources.Clear();
        //            config.SetBasePath(Directory.GetCurrentDirectory());
        //            config.AddJsonFile("appsettings.json", optional: true);
        //            config.AddJsonFile($"appsettings.{host.HostingEnvironment.EnvironmentName}.json", optional: true);
        //            config.AddEnvironmentVariables();

        //            config.AddCommandLine(args);
        //        })
        //        .ConfigureServices((host, services) =>
        //        {
        //            services.AddHostedService<Worker>();
        //            services.AddOptions();

        //            var configurationRoot = host.Configuration;
        //            services.Configure<RoverSettings>(
        //                configurationRoot.GetSection(nameof(RoverSettings)) //.Get<RoverSettings>()
        //            );

        //            //var containerBuilder = new ContainerBuilder();

        //            //var container = EventFlowOptions.New
        //            //    .UseAutofacContainerBuilder(containerBuilder)
        //            //    .AddAspNetCoreMetadataProviders()
        //            //    .AddEvents(typeof(ExampleEvent))
        //            //    .AddCommands(typeof(ExampleCommand))
        //            //    .AddCommandHandlers(typeof(ExampleCommandHandler))
        //            //    .UseConsoleLog()
        //            //    .UseFilesEventStore(FilesEventStoreConfiguration.Create("./evt-store"))
        //            //    .UseInMemoryReadStoreFor<ExampleReadModel>();

        //            //containerBuilder.Populate(services);

        //            services.AddTransient<IWaitService, WaitService>();
        //            //services.AddHostedService<GracePeriodManagerService>()

        //        });
        //}

        public static async Task Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureAppConfiguration((host, config) => {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", true, true);
                    config.AddJsonFile($"appsettings.{host.HostingEnvironment.EnvironmentName}.json", true, true);
                    config.AddEnvironmentVariables();
                })
                .ConfigureServices(
                    (hostcontext, services) => {
                        //var envconfig = EnvironmentConfiguration.Bind(hostcontext.Configuration);
                        //services.AddSingleton(envconfig);

                        services.AddOptions();

                        var configurationRoot = hostcontext.Configuration;
                        services.Configure<RoverSettings>(configurationRoot.GetSection(nameof(RoverSettings)));
                        services.Configure<IntegrationSettings>(configurationRoot.GetSection(nameof(IntegrationSettings)));
                        services.Configure<MarsSettings>(configurationRoot.GetSection(nameof(MarsSettings)));

                        EventFlowOptions.New
                            .Configure(cfg => cfg.IsAsynchronousSubscribersEnabled = true)
                            .UseServiceCollection(services)
                            .AddAspNetCoreMetadataProviders()
                            .PublishToRabbitMq(RabbitMqConfiguration.With(
                                new Uri(configurationRoot.GetSection(nameof(IntegrationSettings)).GetValue<string>("RabbitMQConnectionString")), 
                                true, 5,
                                configurationRoot.GetSection(nameof(IntegrationSettings)).GetValue<string>("RabbitMQPublishExchange")))
                            //.RegisterModule<DomainModule>()

                            .AddEvents(typeof(StoppedEvent))
                            .AddCommands(typeof(StopCommand))
                            .AddCommandHandlers(typeof(StopCommandHandler))
                            .UseInMemoryReadStoreFor<StopReadModel>()

                            .AddEvents(typeof(MovedEvent))
                            .AddCommands(typeof(MoveCommand))
                            .AddCommandHandlers(typeof(MoveCommandHandler))
                            .UseInMemoryReadStoreFor<MoveReadModel>()

                            .AddEvents(typeof(TurnedEvent))
                            .AddCommands(typeof(TurnCommand))
                            .AddCommandHandlers(typeof(TurnCommandHandler))
                            .UseInMemoryReadStoreFor<TurnReadModel>()

                            .AddEvents(typeof(StartEvent))
                            .AddEvents(typeof(PositionChangedEvent))
                            //.AddEvents(typeof(MoveEvent))

                            //
                            // subscribe services changed
                            //
                            .AddAsynchronousSubscriber<StartAggregate, StartId, StartEvent, StartEventSubscriber>()
                            .RegisterServices(s => {
                                s.Register<IHostedService, RabbitConsumePersistenceService>(Lifetime.Singleton);
                                s.Register<IHostedService, StartEventSubscriber>(Lifetime.Singleton);
                            });
                    })
                .ConfigureLogging((hostingContext, logging) => { });

            await builder.RunConsoleAsync();
        }

    }
}