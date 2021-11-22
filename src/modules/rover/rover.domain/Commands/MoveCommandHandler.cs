using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using rover.domain.Aggregates;
using rover.domain.Models;

namespace rover.domain.Commands
{
    public class MoveCommandHandler : CommandHandler<RoverAggregate, RoverAggregateId, IExecutionResult, MoveCommand>
    {
        public override Task<IExecutionResult> ExecuteCommandAsync(
            RoverAggregate aggregate,
            MoveCommand command,
            CancellationToken cancellationToken)
        {
            var executionResult = aggregate.Move(command.Moves, command.IsBlocked);
            return Task.FromResult(executionResult);
        }
    }
}