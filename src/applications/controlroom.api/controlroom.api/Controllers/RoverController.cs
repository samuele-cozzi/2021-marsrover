using EventFlow;
using EventFlow.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using rover.domain.Commands;
using rover.domain.Models;
using rover.domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace controlroom.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoverController : ControllerBase
    {
        private readonly ILogger<RoverController> _logger;
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _queryProcessor;

        public RoverController(
            ILogger<RoverController> logger,
            ICommandBus commandBus,
            IQueryProcessor queryProcessor)
        {
            _logger = logger;
            _commandBus = commandBus;
            _queryProcessor = queryProcessor;
        }

        
        // POST api/<Rover>/move
        [HttpPost("move")]
        public void Move(string[] value)
        {
            var enumList = value
              .Select(x => (Moves)Enum.Parse(typeof(Moves), x)).ToArray();

            _commandBus.PublishAsync(new StartCommand(StartId.New, enumList, true), CancellationToken.None);
        }

        // POST api/<Rover>/explore
        [HttpPost("explore")]
        public void Explore()
        {
            _commandBus.PublishAsync(new StartCommand(StartId.New, new Moves[4] { Moves.f, Moves.f, Moves.f, Moves.f }, false), CancellationToken.None);
        }
    }
}
