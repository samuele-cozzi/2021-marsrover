using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using rover.Services.Interfaces;
using rover.Settings;
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

        private readonly IWaitService _waitService;
        private readonly ILogger<Worker> _logger;
        private readonly RoverSettings _options;

        private int? _exitCode;

        public Worker(IWaitService service, IHostApplicationLifetime hostLifetime, ILogger<Worker> logger, IOptions<RoverSettings> options)
        {
            _waitService = service ?? throw new ArgumentNullException(nameof(service));
            _hostLifetime = hostLifetime ?? throw new ArgumentNullException(nameof(hostLifetime));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Start Worker");
            _logger.LogInformation($"Rover Landing Latidude: {_options.Landing.Latitude}");
            _logger.LogInformation($"Rover Landing Longitude: {_options.Landing.Longitude}");
            _logger.LogInformation($"Rover Landing Orientation: {_options.Landing.Orientation}");

            try
            {
                await _waitService.PerformWaitTask();

                _exitCode = 0;
            }
            catch (OperationCanceledException)
            {
                _logger?.LogInformation("The job has been killed with CTRL+C");
                _exitCode = -1;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "An error occurred");
                _exitCode = 1;
            }
            finally
            {
                _hostLifetime.StopApplication();
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
