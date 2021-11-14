using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using rover.domain.Aggregates;
using rover.domain.Models;

namespace rover.domain.Commands
{
    public class MoveCommandHandler : CommandHandler<RoverPositionAggregate, RoverPositionAggregateId, IExecutionResult, MoveCommand>
    {
        public override Task<IExecutionResult> ExecuteCommandAsync(
            RoverPositionAggregate aggregate,
            MoveCommand command,
            CancellationToken cancellationToken)
        {
            var executionResult = aggregate.Move(command.Moves, command.IsBlocked);
            return Task.FromResult(executionResult);
        }
    }
}