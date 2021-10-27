using EventFlow.Aggregates;
using EventFlow.Aggregates.ExecutionResults;
using Microsoft.Extensions.Options;
using rover.application.DomainEvents;
using rover.application.Entities;
using rover.application.Models;
using rover.domain.AggregateModels.Rover;
using rover.domain.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rover.application.Aggregates
{
    public class MoveAggregate : AggregateRoot<MoveAggregate, MoveId>, IEmit<MovedEvent>
    {
        private readonly MarsSettings _options;

        public MoveAggregate(
            MoveId id,
            IOptions<MarsSettings> options
            ) : base(id) {

            _options ??= options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public IExecutionResult Move(Position position, Moves move)
        {
            if (move == Moves.f)
            {
                if (position.FacingDirection == FacingDirections.N)
                    position.Latitude += 360 / _options.AngularPartition;
                if (position.FacingDirection == FacingDirections.S)
                    position.Latitude -= 360 / _options.AngularPartition;
                if (position.FacingDirection == FacingDirections.E)
                    position.Longitude += 360 / _options.AngularPartition;
                if (position.FacingDirection == FacingDirections.W)
                    position.Longitude -= 360 / _options.AngularPartition;
            }

            if (move == Moves.b)
            {
                if (position.FacingDirection == FacingDirections.N)
                    position.Latitude -= 360 / _options.AngularPartition;
                if (position.FacingDirection == FacingDirections.S)
                    position.Latitude += 360 / _options.AngularPartition;
                if (position.FacingDirection == FacingDirections.E)
                    position.Longitude -= 360 / _options.AngularPartition;
                if (position.FacingDirection == FacingDirections.W)
                    position.Longitude += 360 / _options.AngularPartition;
            }

            var obstacle = _options.Obstacles.FirstOrDefault(z => z.X == position.Longitude && z.Y == position.Latitude);
            if(obstacle == null) { 
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
