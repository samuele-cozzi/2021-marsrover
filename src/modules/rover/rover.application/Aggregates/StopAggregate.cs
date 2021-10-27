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
    public class StopAggregate : AggregateRoot<StopAggregate, StopId>, IEmit<StoppedEvent>
    {
        private Position position;

        public StopAggregate(StopId id) : base(id) { }

        public IExecutionResult Stop(StartId startId, Position landingPosition, bool isBlocked, bool stop)
        {
            //Check if position is on obstacle


            Emit(new StoppedEvent(
                startId.ToString(), landingPosition.FacingDirection, landingPosition.Latitude, landingPosition.Longitude, isBlocked, stop
            ));

            return ExecutionResult.Success();
        }

        public void Apply(StoppedEvent aggregateEvent)
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
