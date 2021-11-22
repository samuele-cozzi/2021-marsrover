using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using rover.domain.Aggregates;
using rover.domain.Models;

namespace rover.domain.Commands
{
    public class StartCommandHandler : CommandHandler<RoverAggregate, RoverAggregateId, IExecutionResult, StartCommand>
    {
        public override Task<IExecutionResult> ExecuteCommandAsync(
            RoverAggregate aggregate,
            StartCommand command,
            CancellationToken cancellationToken)
        {
            var executionResult = aggregate.Start(command.Move, command.Stop);
            return Task.FromResult(executionResult);
        }
    }
}
