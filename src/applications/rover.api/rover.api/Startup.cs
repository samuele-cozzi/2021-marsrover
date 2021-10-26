using Autofac;
using Autofac.Extensions.DependencyInjection;
using EventFlow;
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
using rover.application.Commands;
using rover.application.DomainEvents;
using rover.application.Models;
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

            return EventFlowOptions.New
                .UseServiceCollection(services)
                //.UseAutofacContainerBuilder(containerBuilder)
                .AddEvents(typeof(LandedEvent))
                .AddEvents(typeof(MoveEvent))
                .AddCommands(typeof(LandingCommand))
                .AddCommands(typeof(MoveCommand))
                .AddCommandHandlers(typeof(LandingCommandHandler))
                .AddCommandHandlers(typeof(MoveCommandHandler))
                //.AddSnapshots(typeof(CompetitionSnapshot))
                //.RegisterServices(sr => sr.Register(i => SnapshotEveryFewVersionsStrategy.Default))
                //.RegisterServices(sr => sr.Register(c => ConfigurationManager.ConnectionStrings["EventStore"].ConnectionString))
                //.AddSynchronousSubscriber<MoveAggregate, RoverId, MoveEvent, MoveEventSubscriber>()

                .UseMssqlReadModel<LandingReadModel>()
                .UseMssqlReadModel<MoveReadModel>()
                //.UseInMemoryReadStoreFor<LandingReadModel>()
                //.UseInMemoryReadStoreFor<MoveReadModel>()
                .ConfigureMsSql(MsSqlConfiguration.New.SetConnectionString("Server=localhost,5433;Database=RoverRM;User Id=sa;Password=Pass@word"))
                .PublishToRabbitMq(RabbitMqConfiguration.With(new Uri($"amqp://localhost:5672"), true, 5, "eventflow"))
                //.RegisterServices(s => {
                //    s.Register<IHostedService, MoveEventSubscriber>(Lifetime.Singleton);
                //})
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
