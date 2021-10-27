using System;
using System.Linq;
using EventFlow.Aggregates;
using EventFlow.Aggregates.ExecutionResults;
using Microsoft.Extensions.Options;
using rover.domain.DomainEvents;
using rover.domain.Models;
using rover.domain.Settings;

namespace rover.domain.Aggregates
{
    public class MoveAggregate : AggregateRoot<MoveAggregate, MoveId>, IEmit<MovedEvent>
    {
        private readonly MarsSettings _options;
        private const int circle = 360;

        public MoveAggregate(
            MoveId id,
            IOptions<MarsSettings> options
            ) : base(id) {

            _options ??= options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public IExecutionResult Move(Position position, Moves move)
        {
            double x = position.Longitude, y = position.Latitude;

            if (move == Moves.f)
            {
                if (position.FacingDirection == FacingDirections.N)
                    y += 360 / _options.AngularPartition;
                if (position.FacingDirection == FacingDirections.S)
                    y -= 360 / _options.AngularPartition;
                if (position.FacingDirection == FacingDirections.E)
                    x += 360 / _options.AngularPartition;
                if (position.FacingDirection == FacingDirections.W)
                    x -= 360 / _options.AngularPartition;
            }

            if (move == Moves.b)
            {
                if (position.FacingDirection == FacingDirections.N)
                    y -= 360 / _options.AngularPartition;
                if (position.FacingDirection == FacingDirections.S)
                    y += 360 / _options.AngularPartition;
                if (position.FacingDirection == FacingDirections.E)
                    x -= 360 / _options.AngularPartition;
                if (position.FacingDirection == FacingDirections.W)
                    x += 360 / _options.AngularPartition;
            }

            var obstacle = _options.Obstacles.FirstOrDefault(z => z.X == x && z.Y == y);


            if (obstacle == null)
            {
                position.Longitude = x;
                position.Latitude = y;
                Emit(new MovedEvent(position.Latitude, position.Longitude));

                return ExecutionResult.Success();
            }
            else
            {
                return ExecutionResult.Failed();
            }
        }

        public void Apply(MovedEvent aggregateEvent)
        {
            
        }
    }
}
