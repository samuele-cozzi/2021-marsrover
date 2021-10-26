using EventFlow;
using EventFlow.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using rover.application.Commands;
using rover.application.Models;
using rover.domain.AggregateModels.Rover;
using System.Collections.Generic;
using System.Threading;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace rover.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

        // GET: api/<Rover>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<Rover>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<Rover>
        [HttpPost("move")]
        public void Move(string[] value)
        {
            _commandBus.PublishAsync(new StartCommand(StartId.New, value, true), CancellationToken.None);
        }

        [HttpPost("explore")]
        public void Explore()
        {
            _commandBus.PublishAsync(new StartCommand(StartId.New, new string[4] {"f","f","f","f"}, false), CancellationToken.None);
        }
    }
}
