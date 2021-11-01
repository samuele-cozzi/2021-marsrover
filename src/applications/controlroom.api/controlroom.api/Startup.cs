using controlroom.api.DomainEventsHandlers;
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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using rover.domain.Aggregates;
using rover.domain.Commands;
using rover.domain.DomainEvents;
using rover.domain.Models;
using rover.domain.Queries;
using rover.domain.Settings;
using rover.infrastructure.ef;
using rover.infrastructure.rabbitmq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

namespace controlroom.api
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
            services.AddCors();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "controlroom.api",
                    Description = "A simple example ASP.NET Core Web API",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Shayne Boyer",
                        Email = string.Empty,
                        Url = new Uri("https://twitter.com/spboyer"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("https://example.com/license"),
                    }
                });
        
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.Configure<RoverSettings>(Configuration.GetSection(nameof(RoverSettings)));
            services.Configure<IntegrationSettings>(Configuration.GetSection(nameof(IntegrationSettings)));

            services.AddDbContext<DBContextControlRoom>(options =>
                options.UseSqlServer(Configuration.GetSection(nameof(IntegrationSettings)).GetValue<string>("ReadModelConnectionString")));
            services.AddScoped<IPositionRepository, PositionRepository>();

            return EventFlowOptions.New
                .UseServiceCollection(services)

                .AddEvents(typeof(StartEvent))
                .AddCommands(typeof(StartCommand))
                .AddCommandHandlers(typeof(StartCommandHandler))
                

                .AddEvents(typeof(PositionChangedEvent))
                .AddCommands(typeof(PositionCommand))
                .AddCommandHandlers(typeof(PositionCommandHandler))
                

                .AddEvents(typeof(StoppedEvent))
                .AddEvents(typeof(TurnedEvent))
                .AddEvents(typeof(MovedEvent))

                .RegisterServices(sr => sr.Register(c => Configuration.GetSection(nameof(IntegrationSettings)).GetValue<string>("ReadModelConnectionString")))
                .AddEntityFrameworkReadModel()
                
                .AddQueryHandler<GetPositionsQueryHandler, GetPositionsQuery, List<PositionReadModel>>()
                .AddQueryHandler<GetLastPositionQueryHandler, GetLastPositionQuery, PositionReadModel>()

                //.UseMssqlReadModel<PositionReadModel>()
                //.UseMssqlReadModel<StartReadModel>()
                //.ConfigureMsSql(MsSqlConfiguration.New.SetConnectionString(
                //    Configuration.GetSection(nameof(IntegrationSettings)).GetValue<string>("ReadModelConnectionString")))

                .PublishToRabbitMq(RabbitMqConfiguration.With(
                                new Uri(Configuration.GetSection(nameof(IntegrationSettings)).GetValue<string>("RabbitMQConnectionString")),
                                true, 5,
                                Configuration.GetSection(nameof(IntegrationSettings)).GetValue<string>("RabbitMQPublishExchange")))

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
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "controlroom.api v1"));
            }

            app.UseCors(
                options => options.WithOrigins("*")
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
            );

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
