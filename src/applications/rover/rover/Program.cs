using EventFlow;
using EventFlow.AspNetCore.Extensions;
using EventFlow.Configuration;
using EventFlow.DependencyInjection.Extensions;
using EventFlow.Extensions;
using EventFlow.RabbitMQ;
using EventFlow.RabbitMQ.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using rover.infrastructure.rabbitmq;
using System;
using System.IO;
using System.Threading.Tasks;
using rover.domain.Aggregates;
using rover.domain.Commands;
using rover.domain.DomainEvents;
using rover.domain.Models;
using rover.domain.Settings;
using rover.DomainEventsHandler;
using rover.domain.Queries;

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
                    config.AddEnvironmentVariables();
                })
                .ConfigureServices(
                    (hostcontext, services) => {

                        services.AddOptions();

                        var configurationRoot = hostcontext.Configuration;
                        services.Configure<RoverSettings>(configurationRoot.GetSection(nameof(RoverSettings)));
                        services.Configure<IntegrationSettings>(configurationRoot.GetSection(nameof(IntegrationSettings)));
                        services.Configure<MarsSettings>(configurationRoot.GetSection(nameof(MarsSettings)));

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

                        services.AddHostedService<Worker>();
                    })
                .ConfigureLogging((hostingContext, logging) => { });

            await builder.RunConsoleAsync();
        }

    }
}