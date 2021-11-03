using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using rover.domain.Aggregates;
using rover.domain.Models;

namespace rover.domain.Commands
{
    public class ObstacleCommand : Command<ObstacleAggregate, ObstacleId, IExecutionResult>
    {
        public ObstacleCommand(ObstacleId aggregateId, Coordinate position) : base(aggregateId)
        {
            ObstaclePosition = position;
        }

        public Coordinate ObstaclePosition { get; }
    }
}
