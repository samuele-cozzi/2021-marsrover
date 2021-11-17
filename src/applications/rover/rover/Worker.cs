using EventFlow;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using rover.domain.Settings;
using rover.domain.Commands;
using rover.domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace rover
{
    public class Worker : IHostedService
    {
        private readonly IHostApplicationLifetime _hostLifetime;
        private readonly ILogger<Worker> _logger;
        private readonly RoverSettings _roverSettings;
        private readonly MarsSettings _marsSettings;
        private readonly ICommandBus _commandBus;

        private int? _exitCode;

        public Worker(IHostApplicationLifetime hostLifetime, 
            ILogger<Worker> logger, 
            IOptions<RoverSettings> roverSettings, 
            IOptions<MarsSettings> marsSettings,
            ICommandBus commandBus)
        {
            _hostLifetime = hostLifetime ?? throw new ArgumentNullException(nameof(hostLifetime));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _roverSettings = roverSettings?.Value ?? throw new ArgumentNullException(nameof(roverSettings));
            _marsSettings = marsSettings?.Value ?? throw new ArgumentNullException(nameof(marsSettings));
            _commandBus = commandBus;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Start Worker");
            
            if (_marsSettings.ObstaclesPercentage > 0)
            {
                _marsSettings.Obstacles = new List<Coordinate>();
                double step = 360 / _marsSettings.AngularPartition;
                for (double lat = -90; lat <= 90; lat += step)
                {
                    for (double lon = -180; lon <= 180; lon += step)
                    {
                        if (lat == -90 || lat == 90)
                        {
                            _marsSettings.Obstacles.Add(new Coordinate() { Latitude = lat, Longitude = lon });
                        }
                        else
                        {
                            Random rnd = new Random();
                            if (rnd.NextDouble() < _marsSettings.ObstaclesPercentage)
                            {
                                _marsSettings.Obstacles.Add(new Coordinate() { Latitude = lat, Longitude = lon });
                            }
                        }         
                    }
                }
            }
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Environment.ExitCode = _exitCode.GetValueOrDefault(-1);
            _logger?.LogInformation($"Shutting down the service with code {Environment.ExitCode}");
            return Task.CompletedTask;
        }
    }
}
