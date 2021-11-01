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
using Microsoft.AspNetCore.Http;

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


        /// <summary>
        /// Start move the rover
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/rover/move
        ///     ["f", "l", "r", "b"]
        ///
        /// </remarks>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <response code="200">Ok</response>
        /// <response code="400">value is not well formatted</response> 
        /// <response code="500">Server Error</response> 

        // POST api/<Rover>/move
        [HttpPost("move")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Move(string[] value)
        {
            var eventId = new EventId();
            _logger.LogInformation(eventId, "Move Start Command", value);

            Moves[] enumList;
            try {
                enumList = value
                    .Select(x => (Moves)Enum.Parse(typeof(Moves), x)).ToArray();
            }
            catch (Exception e){
                _logger.LogError(eventId, e, "Bad Request");
                return BadRequest(e);
            }
            
            var result = _commandBus.PublishAsync(new StartCommand(StartId.New, enumList, true), CancellationToken.None).Result;
            if(result.IsSuccess){
                _logger.LogError(eventId, "Error");
                return StatusCode(500);
            }
            else
            {
                _logger.LogInformation(eventId, "Move Start End");
                return Ok();
            }
        }


        /// <summary>
        /// Start explore mars
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/rover/explore
        ///
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Ok</response>
        /// <response code="500">Server Error</response> 

        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        // POST api/<Rover>/explore
        [HttpPost("explore")]
        public async Task<ActionResult>  Explore()
        {
            var eventId = new EventId();
            _logger.LogInformation(eventId, "Start Explore Command");

            var result = _commandBus.PublishAsync(new StartCommand(StartId.New, new Moves[4] { Moves.f, Moves.f, Moves.f, Moves.f }, false), CancellationToken.None).Result;
            if(result.IsSuccess){
                _logger.LogError(eventId, "Error");
                return StatusCode(500);
            }
            else
            {
                _logger.LogInformation(eventId, "End Explore Command");
                return Ok();
            }

            
        }
    }
}
