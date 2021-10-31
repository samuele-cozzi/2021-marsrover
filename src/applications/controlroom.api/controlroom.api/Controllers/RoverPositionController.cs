using System.Collections.Generic;
using System.Threading;
using EventFlow;
using EventFlow.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using rover.domain.Models;
using rover.domain.Queries;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace controlroom.api.Controllers
{
    [Route("api/rover/position")]
    [ApiController]
    public class RoverPositionController : ControllerBase
    {
        private readonly ILogger<RoverController> _logger;
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _queryProcessor;

        public RoverPositionController(
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
        public IEnumerable<PositionReadModel> Get()
        {
            var result = _queryProcessor.ProcessAsync(new GetPositionsQuery(), CancellationToken.None).Result;
            return result;
        }

        // GET api/<Rover>/5
        [HttpGet("last")]
        public PositionReadModel Last()
        {
            var result = _queryProcessor.ProcessAsync(new GetLastPositionQuery(), CancellationToken.None).Result;
            return result;
        }

    }
}
