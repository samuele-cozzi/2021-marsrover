using EventFlow;
using EventFlow.Aggregates;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Queries;
using EventFlow.Jobs;
using Microsoft.Extensions.Options;
using rover.domain.Commands;
using rover.domain.DomainEvents;
using rover.domain.Models;
using rover.domain.Queries;
using rover.domain.Settings;
using rover.domain.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace rover.domain.Aggregates
{
    public  class RoverAggregate : AggregateRoot<RoverAggregate, RoverAggregateId>
    {
        public Position _position { get; private set; }
        public Position _landing { get; private set; }

        private readonly IQueryProcessor _queryProcessor;
        private readonly ICommandBus _commandBus;
        private readonly IJobScheduler _jobScheduler;
        private readonly RoverSettings _roverSettings;
        private readonly MarsSettings _marsSettings;
        private readonly IntegrationSettings _options;
        private readonly double angularStep;


        public RoverAggregate(RoverAggregateId id,
            IQueryProcessor queryProcessor,
            ICommandBus commandBus,
            IJobScheduler jobScheduler,
            IOptions<RoverSettings> roverSettings,
            IOptions<MarsSettings> marsSettings,
            IOptions<IntegrationSettings> options
            ) : base(id)
        {
            _queryProcessor = queryProcessor;
            _commandBus = commandBus;
            _jobScheduler = jobScheduler;
            _roverSettings = roverSettings?.Value ?? throw new ArgumentNullException(nameof(roverSettings));
            _marsSettings = marsSettings?.Value ?? throw new ArgumentNullException(nameof(marsSettings));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            angularStep = (_marsSettings.AngularPartition != 0) ? 360 / _marsSettings.AngularPartition : 0;
        }


        public IExecutionResult Start(Moves[] move, bool stop)
        {
            Emit(new StartedEvent(move, stop));

            return ExecutionResult.Success();
        }

        public IExecutionResult Move(Moves[] moves, bool stop)
        {
            //Business logic to move rover
            _position = _position ?? _roverSettings.Landing;

            bool isBlocked = false;
            double latitude = _position.Coordinate.Latitude;
            double longitude = _position.Coordinate.Longitude;
            var position = new Position()
            {
                FacingDirection = _position.FacingDirection,
                Coordinate = new Coordinate()
                {
                    Latitude = latitude,
                    Longitude = longitude
                }
            };

            foreach (var move in moves)
            {
                switch (move)
                {
                    case Moves.f:
                        position.MoveFarward(angularStep);
                        break;
                    case Moves.b:
                        position.MoveBackward(angularStep);
                        break;
                    case Moves.r:
                        position.TurnRight();
                        break;
                    case Moves.l:
                        position.TurnLeft();
                        break;
                }

                isBlocked = CheckObstacles(position);
                if (!isBlocked)
                {
                    latitude = position.Coordinate.Latitude;
                    longitude = position.Coordinate.Longitude;
                }
                else
                {
                    break;
                }
                
            }

            // Emit Stopped Event
            Emit(new MovedEvent(position.FacingDirection, latitude, longitude, _roverSettings.Landing.Coordinate.AngularPrecision, isBlocked, stop));

            return ExecutionResult.Success();
        }

        private bool CheckObstacles(Position position)
        {
            if(_marsSettings.Obstacles.Count(
                x=> x.Latitude == position.Coordinate.Latitude &&
                x.Longitude == position.Coordinate.Longitude) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public IExecutionResult ChangePosition(Position position, bool isBlocked, bool stop)
        {
            if (_landing == null) { 
                Emit(new LandedEvent(position.FacingDirection, position.Coordinate.Latitude, position.Coordinate.Longitude));
            }

            Emit(new StoppedEvent(position.FacingDirection, position.Coordinate.Latitude, position.Coordinate.Longitude, isBlocked, stop));

            var landing = _queryProcessor.ProcessAsync(new GetLandingPositionQuery(), CancellationToken.None).Result;

            bool continueMoving = !stop;
            bool isOnLandingLongitude = position.Coordinate.Longitude >= (landing?.Longitude - position.Coordinate.AngularPrecision) &&
                position.Coordinate.Longitude <= (landing?.Longitude + position.Coordinate.AngularPrecision);

            if (continueMoving && !isOnLandingLongitude)
            {
                Moves[] moves = new Moves[4];
                if (isBlocked)
                {
                    var rnd = new Random();
                    if (rnd.NextDouble() > 0.5)
                    {
                        moves = new Moves[4] { Moves.r, Moves.f, Moves.l, Moves.f };
                    }
                    else
                    {
                        moves = new Moves[4] { Moves.l, Moves.f, Moves.r, Moves.f };
                    }
                }
                else
                {
                    moves = new Moves[4] { Moves.f, Moves.f, Moves.f, Moves.f };
                }

                _jobScheduler.ScheduleAsync(
                    new SendMessageToRoverJob(this.Id, moves, stop),
                    TimeSpan.FromSeconds(_options.TimeDistanceOfMessageInSeconds),
                    CancellationToken.None)
                    .ConfigureAwait(false);
            }

            return ExecutionResult.Success();
        }

        public void Apply(StartedEvent aggregateEvent)
        {
        }

        public void Apply(MovedEvent aggregateEvent)
        {
            if (_position != null) { 
                _position.Coordinate.Latitude = aggregateEvent.Latitude;
                _position.Coordinate.Longitude = aggregateEvent.Longitude;
                _position.FacingDirection = aggregateEvent.FacingDirection;
            }
        }

        public void Apply(LandedEvent aggregateEvent)
        {
            _landing = _landing ?? new Position() { Coordinate = new Coordinate() };
            _landing.Coordinate.Latitude = aggregateEvent.Latitude;
            _landing.Coordinate.Longitude = aggregateEvent.Longitude;
            _landing.FacingDirection = aggregateEvent.FacingDirection;
        }

        public void Apply(StoppedEvent aggregateEvent)
        {
            _position = _position ?? new Position() { Coordinate = new Coordinate() };
            _position.Coordinate.Latitude = aggregateEvent.Latitude;
            _position.Coordinate.Longitude = aggregateEvent.Longitude;
            _position.FacingDirection = aggregateEvent.FacingDirection;
        }
    }
}
