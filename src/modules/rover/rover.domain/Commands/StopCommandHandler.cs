using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using rover.domain.Aggregates;
using rover.domain.Models;

namespace rover.domain.Commands
{
    public class StopCommandHandler : CommandHandler<StopAggregate, StopId, IExecutionResult, StopCommand>
    {
        public override Task<IExecutionResult> ExecuteCommandAsync(
            StopAggregate aggregate,
            StopCommand command,
            CancellationToken cancellationToken)
        {
            var executionResult = aggregate.Stop(command.StartId, command.RoverPosition, command.IsBlocked, command.Stop);
            return Task.FromResult(executionResult);
        }
    }
}
