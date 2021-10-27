using EventFlow.Aggregates;
using EventFlow.Aggregates.ExecutionResults;
using rover.domain.DomainEvents;
using rover.domain.Models;

namespace rover.domain.Aggregates
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
