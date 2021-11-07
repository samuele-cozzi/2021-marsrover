using EventFlow.Aggregates;
using EventFlow.Aggregates.ExecutionResults;
using rover.domain.DomainEvents;
using rover.domain.Models;

namespace rover.domain.Aggregates
{
    public class StopAggregate : AggregateRoot<StopAggregate, StopId>, IEmit<StoppedEvent>
    {
        private Position position;

        public StopAggregate(StopId id) : base(id) { }

        public IExecutionResult Stop(StartId startId, Position landingPosition, bool isBlocked, bool stop)
        {
            //Check if position is on obstacle


            Emit(new StoppedEvent(
                startId.ToString(), landingPosition.FacingDirection, landingPosition.Coordinate.Latitude, landingPosition.Coordinate.Longitude, landingPosition.Coordinate.Precision, isBlocked, stop
            ));

            return ExecutionResult.Success();
        }

        public void Apply(StoppedEvent aggregateEvent)
        {
            position = new Position()
            {
                Coordinate = new Coordinate() {
                    Latitude = aggregateEvent.Latitude,
                    Longitude = aggregateEvent.Longitude
                },
                FacingDirection = aggregateEvent.FacingDirection
            };
        }
    }
}
