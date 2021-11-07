using System.Collections.Generic;
using System.Threading;
using EventFlow;
using EventFlow.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using rover.domain.Models;
using rover.domain.Queries;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Linq;

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


        /// <summary>
        /// Get list of rover position
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/rover/position
        ///
        /// </remarks>
        /// <returns>list of rover positions</returns>
        /// <response code="200">Ok</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        // GET: api/<Rover>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PositionReadModel>>> Get()
        {
            var eventId = new EventId();
            _logger.LogInformation(eventId, "start - Get: rover position");

            var result = await _queryProcessor.ProcessAsync(new GetPositionsQuery(), CancellationToken.None);

            _logger.LogInformation(eventId, "end - Get: rover position");
            return result.OrderByDescending(x => x.Timestamp).ToList();
        }
        
        /// <summary>
        /// Get last recorder rover posotion
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/rover/position/last
        ///
        /// </remarks>
        /// <returns>last registered rover positions</returns>
        /// <response code="200">Ok</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        // GET api/<Rover>/5
        [HttpGet("last")]
        public async Task<ActionResult<PositionReadModel>> Last()
        {
            var eventId = new EventId();
            _logger.LogInformation(eventId, "start - Get last: rover position");

            var result = await _queryProcessor.ProcessAsync(new GetLastPositionQuery(), CancellationToken.None);

            _logger.LogInformation(eventId, "end - Get last: rover position");
            return result;
        }

    }
}
