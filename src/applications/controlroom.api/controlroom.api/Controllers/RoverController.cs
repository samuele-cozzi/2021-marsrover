using EventFlow;
using EventFlow.Queries;
using EventFlow.Jobs;
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
using EventFlow.Provided.Jobs;
using EventFlow.Configuration;
using rover.domain.Settings;
using Microsoft.Extensions.Options;
using rover.domain.Aggregates;

namespace controlroom.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoverController : ControllerBase
    {
        private readonly ILogger<RoverController> _logger;
        private readonly IJobScheduler _jobScheduler;
        private readonly IQueryProcessor _queryProcessor;
        private readonly IntegrationSettings _options;

        public RoverController(
            ILogger<RoverController> logger,
            IJobScheduler jobScheduler,
            IQueryProcessor queryProcessor,
            IOptions<IntegrationSettings> options)
        {
            _logger = logger;
            _jobScheduler = jobScheduler;
            _queryProcessor = queryProcessor;
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        /// <summary>
        /// Rover take off to mars
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/rover/takeoff
        ///
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Ok</response>
        /// <response code="500">Server Error</response> 

        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        // POST api/<Rover>/explore
        [HttpPost("takeoff")]
        public async Task<ActionResult>  TakeOff()
        {
            var eventId = new EventId();
            _logger.LogInformation(eventId, "Takeoff Explore Command");

            var lastPosition = await _queryProcessor.ProcessAsync(new GetLastPositionQuery(), CancellationToken.None);
            RoverPositionAggregateId id = (lastPosition?.AggregateId == null) ? RoverPositionAggregateId.New : RoverPositionAggregateId.With(lastPosition.AggregateId);

            var result = await _jobScheduler.ScheduleAsync(
                new SendMessageToRoverJob(id, new Moves[0] { }, true),
                TimeSpan.FromSeconds(_options.TimeDistanceOfVoyageInSeconds),
                CancellationToken.None)
                .ConfigureAwait(false);

            if(result.Value == null){
                _logger.LogError(eventId, "Error");
                return StatusCode(500);
            }
            else
            {
                _logger.LogInformation(eventId, "End Takeoff Command");
                return Ok();
            }
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

            var lastPosition = await _queryProcessor.ProcessAsync(new GetLastPositionQuery(), CancellationToken.None);
            RoverPositionAggregateId id = RoverPositionAggregateId.With(lastPosition?.AggregateId) ?? RoverPositionAggregateId.New;

            var result = await _jobScheduler.ScheduleAsync(
                new SendMessageToRoverJob(id, enumList, true),
                TimeSpan.FromSeconds(_options.TimeDistanceOfMessageInSeconds),
                CancellationToken.None)
                .ConfigureAwait(false);

            //var result = await _commandBus.PublishAsync(new StartCommand(StartId.New, enumList, true), CancellationToken.None);
            if(result?.Value == null){
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

            var lastPosition = await _queryProcessor.ProcessAsync(new GetLastPositionQuery(), CancellationToken.None);
            RoverPositionAggregateId id = RoverPositionAggregateId.With(lastPosition?.AggregateId) ?? RoverPositionAggregateId.New;

            var result = await _jobScheduler.ScheduleAsync(
                new SendMessageToRoverJob(id, new Moves[4] { Moves.f, Moves.f, Moves.f, Moves.f }, false),
                TimeSpan.FromSeconds(_options.TimeDistanceOfMessageInSeconds),
                CancellationToken.None)
                .ConfigureAwait(false);

            // var result = await _commandBus.PublishAsync(new StartCommand(StartId.New, new Moves[4] { Moves.f, Moves.f, Moves.f, Moves.f }, false), CancellationToken.None);
            if(result.Value == null){
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
