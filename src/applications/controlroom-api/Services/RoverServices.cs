namespace controlroom_api.Services
{
    public class RoverServices
    {
        private readonly ILogger<RoverServices> _logger;
        private readonly IJobScheduler _jobScheduler;
        private readonly IQueryProcessor _queryProcessor;
        private readonly IntegrationSettings _options;

        public RoverServices(
            ILogger<RoverServices> logger,
            IJobScheduler jobScheduler,
            IQueryProcessor queryProcessor,
            IOptions<IntegrationSettings> options)
        {
            _logger = logger;
            _jobScheduler = jobScheduler;
            _queryProcessor = queryProcessor;
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<PositionReadModel> GetLastPosition()
        {
            _logger.LogInformation("start - Get last: rover position");

            var result = await _queryProcessor.ProcessAsync(new GetLastPositionQuery(), CancellationToken.None);

            _logger.LogInformation("end - Get last: rover position");
            return result;
        }

        public async Task<LandingReadModel> GetLandingPosition()
        {
            _logger.LogInformation("start - Get last: rover position");

            var result = await _queryProcessor.ProcessAsync(new GetLandingPositionQuery(), CancellationToken.None);

            _logger.LogInformation("end - Get last: rover position");
            return result;
        }

        public async Task<IEnumerable<PositionReadModel>> Get()
        {
            _logger.LogInformation("start - Get: rover position");

            var result = await _queryProcessor.ProcessAsync(new GetPositionsQuery(), CancellationToken.None);

            _logger.LogInformation("end - Get: rover position");
            return result.OrderByDescending(x => x.Timestamp).ToList();
        }

        public async Task<IResult> TakeOff()
        {
            _logger.LogInformation("Takeoff Explore Command");

            var lastPosition = await _queryProcessor.ProcessAsync(new GetLastPositionQuery(), CancellationToken.None);
            RoverAggregateId id = (lastPosition?.AggregateId == null) ? RoverAggregateId.New : RoverAggregateId.With(lastPosition.AggregateId);

            var result = await _jobScheduler.ScheduleAsync(
                new SendMessageToRoverJob(id, new Moves[0] { }, true),
                TimeSpan.FromSeconds(_options.TimeDistanceOfVoyageInSeconds),
                CancellationToken.None)
                .ConfigureAwait(false);

            if (result.Value == null)
            {
                _logger.LogError("Error");
                return Results.Problem();
            }
            else
            {
                _logger.LogInformation("End Takeoff Command");
                return Results.Ok(); ;
            }
        }

        public async Task<IResult> Move(string[] value)
        {
            _logger.LogInformation("Move Start Command", value);

            Moves[] enumList;
            try
            {
                enumList = value
                    .Select(x => (Moves)Enum.Parse(typeof(Moves), x)).ToArray();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Bad Request");
                return Results.BadRequest(e);
            }

            var lastPosition = await _queryProcessor.ProcessAsync(new GetLastPositionQuery(), CancellationToken.None);
            RoverAggregateId id = RoverAggregateId.With(lastPosition?.AggregateId) ?? RoverAggregateId.New;

            var result = await _jobScheduler.ScheduleAsync(
                new SendMessageToRoverJob(id, enumList, true),
                TimeSpan.FromSeconds(_options.TimeDistanceOfMessageInSeconds),
                CancellationToken.None)
                .ConfigureAwait(false);

            if (result?.Value == null)
            {
                _logger.LogError("Error");
                return Results.Problem();
            }
            else
            {
                _logger.LogInformation("Move Start End");
                return Results.Ok();
            }
        }

        public async Task<IResult> Explore()
        {
            _logger.LogInformation("Start Explore Command");

            var lastPosition = await _queryProcessor.ProcessAsync(new GetLastPositionQuery(), CancellationToken.None);
            RoverAggregateId id = RoverAggregateId.With(lastPosition?.AggregateId) ?? RoverAggregateId.New;

            var result = await _jobScheduler.ScheduleAsync(
                new SendMessageToRoverJob(id, new Moves[4] { Moves.f, Moves.f, Moves.f, Moves.f }, false),
                TimeSpan.FromSeconds(_options.TimeDistanceOfMessageInSeconds),
                CancellationToken.None)
                .ConfigureAwait(false);

            // var result = await _commandBus.PublishAsync(new StartCommand(StartId.New, new Moves[4] { Moves.f, Moves.f, Moves.f, Moves.f }, false), CancellationToken.None);
            if (result.Value == null)
            {
                _logger.LogError("Error");
                return Results.Problem();
            }
            else
            {
                _logger.LogInformation("End Explore Command");
                return Results.Ok();
            }


        }
    }
}
