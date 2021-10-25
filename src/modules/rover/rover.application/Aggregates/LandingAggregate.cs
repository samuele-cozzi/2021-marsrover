using EventFlow.Aggregates;
using EventFlow.Aggregates.ExecutionResults;
using rover.application.DomainEvents;
using rover.application.Entities;
using rover.domain.AggregateModels.Rover;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rover.application.Aggregates
{
    public class LandingAggregate : AggregateRoot<LandingAggregate, RoverId>, IEmit<LandedEvent>
    {
        private Position position;

        public LandingAggregate(RoverId id) : base(id) { }

        public IExecutionResult Land(Position landingPosition)
        {
            //Check if position is on obstacle

            
            Emit(new LandedEvent(
                landingPosition.FacingDirection, landingPosition.Longitude, landingPosition.Latitude
            ));

            return ExecutionResult.Success();
        }

        public void Apply(LandedEvent aggregateEvent)
        {
            position = new Position()
            {
                Latitude = aggregateEvent.Latitude,
                Longitude = aggregateEvent.Longitude,
                FacingDirection = aggregateEvent.FacingDirection
            };
        }
    }
}
