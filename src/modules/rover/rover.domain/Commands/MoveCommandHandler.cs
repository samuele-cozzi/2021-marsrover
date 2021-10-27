using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using rover.domain.Aggregates;
using rover.domain.Models;

namespace rover.domain.Commands
{
    public class MoveCommandHandler : CommandHandler<MoveAggregate, MoveId, IExecutionResult, MoveCommand>
    {
        public override Task<IExecutionResult> ExecuteCommandAsync(
            MoveAggregate aggregate,
            MoveCommand command,
            CancellationToken cancellationToken)
        {
            var executionResult = aggregate.Move(command.Position, command.Move);
            return Task.FromResult(executionResult);
        }
    }
}