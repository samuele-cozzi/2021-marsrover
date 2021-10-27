using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using rover.domain.Aggregates;
using rover.domain.Models;

namespace rover.domain.Commands
{
    public class TurnCommandHandler : CommandHandler<TurnAggregate, TurnId, IExecutionResult, TurnCommand>
    {
        public override Task<IExecutionResult> ExecuteCommandAsync(
            TurnAggregate aggregate,
            TurnCommand command,
            CancellationToken cancellationToken)
        {
            var executionResult = aggregate.Turn(command.FacingDirection, command.Move);
            return Task.FromResult(executionResult);
        }
    }
}
