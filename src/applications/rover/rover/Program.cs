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
using rover.infrastructure.rabbitmq;
using Serilog;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using rover.domain.Aggregates;
using rover.domain.Commands;
using rover.domain.DomainEvents;
using rover.domain.Models;
using rover.domain.Settings;
using rover.DomainEventsHandler;

namespace rover
{
    internal class Program
    {
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