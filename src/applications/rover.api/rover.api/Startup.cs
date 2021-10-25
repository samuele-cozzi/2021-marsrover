using Autofac;
using Autofac.Extensions.DependencyInjection;
using EventFlow;
using EventFlow.Autofac.Extensions;
using EventFlow.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using rover.api.Example;
using rover.application.Commands;
using rover.application.DomainEvents;
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

            var containerBuilder = new ContainerBuilder();

            var container = EventFlowOptions.New
                .UseAutofacContainerBuilder(containerBuilder)
                //.AddAspNetCoreMetadataProviders()
                .AddEvents(typeof(ExampleEvent))
                .AddCommands(typeof(ExampleCommand))
                .AddCommandHandlers(typeof(ExampleCommandHandler))
                .AddEvents(typeof(LandedEvent))
                .AddCommands(typeof(LandingCommand))
                .AddCommandHandlers(typeof(LandingCommandHandler))
                .UseConsoleLog();
            //.UseFilesEventStore(FilesEventStoreConfiguration.Create("./evt-store"))
            //.UseInMemoryReadStoreFor<ExampleReadModel>();

            containerBuilder.Populate(services);

            return new AutofacServiceProvider(containerBuilder.Build());

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
