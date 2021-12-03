using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using rover.domain.Models;
using rover.domain.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace rover.domain.Services
{
    public class InitMarsService
    {
        private readonly ILogger<InitMarsService> _logger;
        private readonly MarsSettings _marsSettings;

        private int? _exitCode;

        public InitMarsService(
            ILogger<InitMarsService> logger,
            IOptions<MarsSettings> marsSettings)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _marsSettings = marsSettings?.Value ?? throw new ArgumentNullException(nameof(marsSettings));
        }

        public Task InitMarsAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Start Init Mars");

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

            _logger.LogInformation($"Finish Init Mars");

            return Task.CompletedTask;
        }
    }
}
