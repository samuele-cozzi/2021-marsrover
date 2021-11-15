using EventFlow;
using EventFlow.Aggregates;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Queries;
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
    public  class RoverPositionAggregate : AggregateRoot<RoverPositionAggregate, RoverPositionAggregateId>
    {
        public Position _position { get; private set; }

        private readonly IQueryProcessor _queryProcessor;
        private readonly ICommandBus _commandBus;
        private readonly RoverSettings _roverSettings;
        private readonly MarsSettings _marsSettings;
        private readonly double angularStep;


        public RoverPositionAggregate(RoverPositionAggregateId id,
            IQueryProcessor queryProcessor,
            ICommandBus commandBus,
            IOptions<RoverSettings> roverSettings,
            IOptions<MarsSettings> marsSettings
            ) : base(id)
        {
            _queryProcessor = queryProcessor;
            _commandBus = commandBus;
            _roverSettings = roverSettings?.Value ?? throw new ArgumentNullException(nameof(roverSettings));
            _marsSettings = marsSettings?.Value ?? throw new ArgumentNullException(nameof(marsSettings));
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
                        position.FacingDirection = position.FacingDirection.Next();
                        break;
                    case Moves.l:
                        position.FacingDirection = position.FacingDirection.Previous();
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
            Emit(new StoppedEvent(position.FacingDirection, latitude, longitude, _roverSettings.Landing.Coordinate.AngularPrecision, isBlocked, stop));

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
            Emit(new PositionChangedEvent(position.FacingDirection, position.Coordinate.Latitude, position.Coordinate.Longitude, isBlocked, stop));

            var landing = _queryProcessor.ProcessAsync(new GetLandingPositionQuery(), CancellationToken.None).Result;

            bool continueMoving = !stop;
            bool isOnLandingLongitude = position.Coordinate.Longitude >= (landing?.Longitude - position.Coordinate.AngularPrecision) &&
                position.Coordinate.Longitude <= (landing?.Longitude + position.Coordinate.AngularPrecision);

            if (continueMoving && !isOnLandingLongitude)
            {
                if (isBlocked)
                {
                    var rnd = new Random();
                    if (rnd.NextDouble() > 0.5)
                    {
                        _commandBus.PublishAsync(
                        new StartCommand(this.Id, new Moves[4] { Moves.r, Moves.f, Moves.l, Moves.f }, stop), CancellationToken.None);
                    }
                    else
                    {
                        _commandBus.PublishAsync(
                        new StartCommand(this.Id, new Moves[4] { Moves.l, Moves.f, Moves.r, Moves.f }, stop), CancellationToken.None);
                    }
                }
                else
                {
                    _commandBus.PublishAsync(
                    new StartCommand(this.Id, new Moves[4] { Moves.f, Moves.f, Moves.f, Moves.f }, stop), CancellationToken.None);
                }
            }

            return ExecutionResult.Success();
        }

        public void Apply(StartedEvent aggregateEvent)
        {
        }

        public void Apply(StoppedEvent aggregateEvent)
        {
            if (_position != null) { 
                _position.Coordinate.Latitude = aggregateEvent.Latitude;
                _position.Coordinate.Longitude = aggregateEvent.Longitude;
                _position.FacingDirection = aggregateEvent.FacingDirection;
            }
        }

        public void Apply(PositionChangedEvent aggregateEvent)
        {
            if (_position != null)
            {
                _position.Coordinate.Latitude = aggregateEvent.Latitude;
                _position.Coordinate.Longitude = aggregateEvent.Longitude;
                _position.FacingDirection = aggregateEvent.FacingDirection;
            }
        }
    }
}
