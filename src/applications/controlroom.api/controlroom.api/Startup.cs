using controlroom.api.DomainEventsHandlers;
using EventFlow;
using EventFlow.Configuration;
using EventFlow.DependencyInjection.Extensions;
using EventFlow.Extensions;
using EventFlow.RabbitMQ;
using EventFlow.RabbitMQ.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
using System.Reflection;
using System.IO;
using Serilog;
using Hangfire;
using Hangfire.SqlServer;
using EventFlow.Hangfire.Extensions;
using EventFlow.EntityFramework;
using rover.domain.Services;
using System.Data.Common;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using EventFlow.MetadataProviders;
using EventFlow.EventStores.EventStore.Extensions;

namespace controlroom.api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
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

            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("JobsConnection"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));

            // Add the processing server as IHostedService
            services.AddHangfireServer();

            //services.AddDbContext<DBContextControlRoom>(options =>
            //    options.UseSqlServer(Configuration.GetConnectionString("ReadModelsConnection")));
            //services.AddScoped<IPositionRepository, PositionRepository>();

            return EventFlowOptions.New
                .UseServiceCollection(services)

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
                    //sr.<DBContextControlRoom>();
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
                .AddJobs(typeof(SendMessageToRoverJob))
                .AddJobs(typeof(HandlingRoverMessagesJob))

                .PublishToRabbitMq(RabbitMqConfiguration.With(
                                new Uri(Configuration.GetSection(nameof(IntegrationSettings)).GetValue<string>("RabbitMQConnectionString")),
                                true, 5,
                                Configuration.GetSection(nameof(IntegrationSettings)).GetValue<string>("RabbitMQPublishExchange")))

                .AddAsynchronousSubscriber<RoverAggregate, RoverAggregateId, MovedEvent, StoppedEventSubscriber>()
                .RegisterServices(s => {
                    s.Register<IHostedService, RabbitConsumePersistenceService>(Lifetime.Singleton);
                    s.Register<IHostedService, StoppedEventSubscriber>(Lifetime.Singleton);

                })
                .UseConsoleLog()
                .CreateServiceProvider();
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
            app.UseHangfireDashboard();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHangfireDashboard();
            });
        }


    }
}
