using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using rover.domain.Aggregates;
using rover.domain.Models;

namespace rover.domain.Commands
{
    public class ObstacleCommandHandler : CommandHandler<ObstacleAggregate, ObstacleId, IExecutionResult, ObstacleCommand>
    {
        public override Task<IExecutionResult> ExecuteCommandAsync(
            ObstacleAggregate aggregate,
            ObstacleCommand command,
            CancellationToken cancellationToken)
        {
            var executionResult = aggregate.CreateObstacle(command.ObstaclePosition);
            return Task.FromResult(executionResult);
        }
    }
}
