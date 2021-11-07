using EventFlow;
using EventFlow.Configuration;
using EventFlow.Extensions;
using EventFlow.MsSql;
using EventFlow.MsSql.Extensions;
using EventFlow.Queries;
using EventFlow.RabbitMQ;
using EventFlow.RabbitMQ.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using rover.domain.Settings;
using rover.domain.Commands;
using rover.domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Start Worker");
            
            if (_marsSettings.ObstaclesPercentage > 0)
            {
                double step = 360 / _marsSettings.AngularPartition;
                for (double lat = -90; lat <= 90; lat += step)
                {
                    for (double lon = -180; lon <= 180; lon += step)
                    {
                        if (lat == -90 || lat == 90)
                        {
                            var command = new ObstacleCommand(ObstacleId.New, new Coordinate() { Latitude = lat, Longitude = lon });
                            _commandBus.PublishAsync(command, CancellationToken.None).Wait();
                        }
                        else
                        {
                            Random rnd = new Random();
                            if (rnd.NextDouble() < _marsSettings.ObstaclesPercentage)
                            {
                                var command = new ObstacleCommand(ObstacleId.New, new Coordinate() { Latitude = lat, Longitude = lon });
                                _commandBus.PublishAsync(command, CancellationToken.None).Wait();
                            }
                        }         
                    }
                }
            }
            else
            {
                foreach (var obstacle in _marsSettings.Obstacles)
                {
                    var command = new ObstacleCommand(ObstacleId.New, new Coordinate() { Latitude = obstacle.Latitude, Longitude = obstacle.Longitude });
                    _commandBus.PublishAsync(command, CancellationToken.None).Wait();
                }
            }

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Environment.ExitCode = _exitCode.GetValueOrDefault(-1);
            _logger?.LogInformation($"Shutting down the service with code {Environment.ExitCode}");
            return Task.CompletedTask;
        }
    }
}
