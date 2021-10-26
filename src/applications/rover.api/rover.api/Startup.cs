using Autofac;
using Autofac.Extensions.DependencyInjection;
using EventFlow;
using EventFlow.Configuration;
using EventFlow.DependencyInjection.Extensions;
using EventFlow.Extensions;
using EventFlow.MsSql;
using EventFlow.MsSql.Extensions;
using EventFlow.RabbitMQ;
using EventFlow.RabbitMQ.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using rover.application.Aggregates;
using rover.application.Commands;
using rover.application.DomainEvents;
using rover.application.Models;
using rover.domain.Settings;
using rover.infrastructure.rabbitmq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rover.api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.Configure<RoverSettings>(Configuration.GetSection(nameof(RoverSettings)));
            services.Configure<IntegrationSettings>(Configuration.GetSection(nameof(IntegrationSettings)));

            return EventFlowOptions.New
                .UseServiceCollection(services)
                //.UseAutofacContainerBuilder(containerBuilder)
                .AddEvents(typeof(MoveEvent))
                .AddCommands(typeof(MoveCommand))
                .AddCommandHandlers(typeof(MoveCommandHandler))
                .UseMssqlReadModel<MoveReadModel>()

                .AddEvents(typeof(StartEvent))
                .AddCommands(typeof(StartCommand))
                .AddCommandHandlers(typeof(StartCommandHandler))
                .UseMssqlReadModel<StartReadModel>()

                .AddEvents(typeof(PositionChangedEvent))
                .AddCommands(typeof(PositionCommand))
                .AddCommandHandlers(typeof(PositionCommandHandler))
                .UseMssqlReadModel<PositionReadModel>()

                //.AddSnapshots(typeof(CompetitionSnapshot))
                //.RegisterServices(sr => sr.Register(i => SnapshotEveryFewVersionsStrategy.Default))
                //.RegisterServices(sr => sr.Register(c => ConfigurationManager.ConnectionStrings["EventStore"].ConnectionString))
                //.AddSynchronousSubscriber<MoveAggregate, RoverId, MoveEvent, MoveEventSubscriber>()

                .AddEvents(typeof(StoppedEvent))

                
                
                //.UseInMemoryReadStoreFor<LandingReadModel>()
                //.UseInMemoryReadStoreFor<MoveReadModel>()
                .ConfigureMsSql(MsSqlConfiguration.New.SetConnectionString(
                    Configuration.GetSection(nameof(IntegrationSettings)).GetValue<string>("ReadModelConnectionString")))
                .PublishToRabbitMq(RabbitMqConfiguration.With(
                                new Uri(Configuration.GetSection(nameof(IntegrationSettings)).GetValue<string>("RabbitMQConnectionString")),
                                true, 5,
                                Configuration.GetSection(nameof(IntegrationSettings)).GetValue<string>("RabbitMQPublishExchange")))
                //.RegisterServices(s => {
                //    s.Register<IHostedService, MoveEventSubscriber>(Lifetime.Singleton);
                //})
                .AddAsynchronousSubscriber<StopAggregate, StopId, StoppedEvent, StoppedEventSubscriber>()
                .RegisterServices(s => {
                    s.Register<IHostedService, RabbitConsumePersistenceService>(Lifetime.Singleton);
                    s.Register<IHostedService, StoppedEventSubscriber>(Lifetime.Singleton);
                    
                })
                .UseConsoleLog().CreateServiceProvider();

            

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
