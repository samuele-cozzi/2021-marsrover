using EventFlow;
using EventFlow.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using rover.api.Domain.Rover;
using rover.application.Commands;
using rover.application.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace rover.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _queryProcessor;

        public WeatherForecastController(
            ILogger<WeatherForecastController> logger,
            ICommandBus commandBus,
            IQueryProcessor queryProcessor)
        {
            _logger = logger;
            _commandBus = commandBus;
            _queryProcessor = queryProcessor;
        }

        [HttpGet]
        public async void Get(int id)
        {
            //var exampleCommand = new ExampleCommand(ExampleId.New, id);

            //await _commandBus.PublishAsync(exampleCommand, CancellationToken.None);

            //var landingCommand = new LandingCommand(RoverId.New, new Position() { FacingDirection = "N", Latitude = 1, Longitude = 2});
            var landingCommand = new MoveCommand(RoverId.New, new string[3] {"f","f","l"});

            _commandBus.PublishAsync(landingCommand, CancellationToken.None);
        }
    }
}
