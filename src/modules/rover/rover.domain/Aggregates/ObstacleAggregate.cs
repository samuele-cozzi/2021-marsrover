using EventFlow.Aggregates;
using EventFlow.Aggregates.ExecutionResults;
using rover.domain.DomainEvents;
using rover.domain.Models;

namespace rover.domain.Aggregates
{
    public class ObstacleAggregate : AggregateRoot<ObstacleAggregate, ObstacleId>, IEmit<ObstacleEvent>
    {
        public ObstacleAggregate(ObstacleId id) : base(id) { }

        public IExecutionResult CreateObstacle(Coordinate obstaclePosition)
        {
            
            Emit(new ObstacleEvent(obstaclePosition.Latitude, obstaclePosition.Longitude));

            return ExecutionResult.Success();
        }

        public void Apply(ObstacleEvent aggregateEvent)
        {
        }
    }
}
