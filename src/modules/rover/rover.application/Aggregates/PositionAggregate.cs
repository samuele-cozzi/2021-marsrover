using EventFlow.Aggregates;
using EventFlow.Aggregates.ExecutionResults;
using rover.application.DomainEvents;
using rover.application.Models;
using rover.domain.AggregateModels.Rover;
using System;
using System.Collections.Generic;
using System.Text;

namespace rover.application.Aggregates
{
    public class PositionAggregate : AggregateRoot<PositionAggregate, PositionId>, IEmit<PositionChangedEvent>
    {
        private Position position;

        public PositionAggregate(PositionId id) : base(id) { }

        public IExecutionResult ChangePosition(Position landingPosition, bool isBlocked, string startId, bool stop)
        {
            //Check if position is on obstacle


            Emit(new PositionChangedEvent(
                landingPosition.FacingDirection, landingPosition.Latitude, landingPosition.Longitude, isBlocked, startId, stop
            ));

            return ExecutionResult.Success();
        }

        public void Apply(PositionChangedEvent aggregateEvent)
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
