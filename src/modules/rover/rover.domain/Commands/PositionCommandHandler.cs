using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using rover.domain.Aggregates;
using rover.domain.Models;

namespace rover.domain.Commands
{
    public class PositionCommandHandler : CommandHandler<PositionAggregate, PositionId, IExecutionResult, PositionCommand>
    {
        public override Task<IExecutionResult> ExecuteCommandAsync(
            PositionAggregate aggregate,
            PositionCommand command,
            CancellationToken cancellationToken)
        {
            var executionResult = aggregate.ChangePosition(command.RoverPosition, command.IsBlocked, command.StartId, command.Stop);
            return Task.FromResult(executionResult);
        }
    }
}
