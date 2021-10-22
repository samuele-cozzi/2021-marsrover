using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using rover.Services.Interfaces;
using rover.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rover.Services
{
    internal class WaitService : IWaitService
    {
        private readonly RoverSettings _options;
        private readonly ILogger<WaitService> _logger;

        public WaitService(IOptions<RoverSettings> options, ILogger<WaitService> logger)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task PerformWaitTask()
        {
            _logger.LogInformation("inizio attesa");
            await Task.Delay(_options.Distance);
            _logger.LogInformation("fine attesa");
        }
    }
}
