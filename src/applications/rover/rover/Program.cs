using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using rover.Services;
using rover.Services.Interfaces;
using rover.Settings;
using Serilog;
using System;
using System.IO;

namespace rover
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Setup Host
            var host = CreateHostBuilder(args).Build();

            //// Invoke Worker
            //using IServiceScope serviceScope = host.Services.CreateScope();
            //IServiceProvider provider = serviceScope.ServiceProvider;

            //Console.WriteLine("Hello World!");

            host.Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                //.UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .UseSerilog((host, loggerConfiguration) =>
                {
                    loggerConfiguration.ReadFrom.Configuration(host.Configuration);
                })
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureAppConfiguration((host, config) =>
                {
                    config.Sources.Clear();
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", optional: true);
                    config.AddJsonFile($"appsettings.{host.HostingEnvironment.EnvironmentName}.json", optional: true);
                    config.AddEnvironmentVariables();

                    config.AddCommandLine(args);
                })
                .ConfigureServices((host, services) =>
                {
                    services.AddHostedService<Worker>();
                    services.AddOptions();

                    var configurationRoot = host.Configuration;
                    services.Configure<RoverSettings>(configurationRoot.GetSection(nameof(RoverSettings)));

                    services.AddTransient<IWaitService, WaitService>();
                    //services.AddHostedService<GracePeriodManagerService>()
                });
        }
    }
}
