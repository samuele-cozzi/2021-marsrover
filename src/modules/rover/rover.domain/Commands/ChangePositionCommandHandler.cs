using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using rover.domain.Aggregates;
using rover.domain.Models;

namespace rover.domain.Commands
{
    public class ChangePositionCommandHandler : CommandHandler<RoverPositionAggregate, RoverPositionAggregateId, IExecutionResult, ChangePositionCommand>
    {
        public override Task<IExecutionResult> ExecuteCommandAsync(
            RoverPositionAggregate aggregate,
            ChangePositionCommand command,
            CancellationToken cancellationToken)
        {
            var executionResult = aggregate.ChangePosition(command.RoverPosition, command.IsBlocked,command.Stop);
            return Task.FromResult(executionResult);
        }
    }
}
