using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using rover.api.Application.Rover.Aggregates;
using System.Threading;
using System.Threading.Tasks;

namespace rover.api.Application.Rover.Commands
{
    public class LandingCommandHandler : CommandHandler<LandingAggregate, RoverId, IExecutionResult, LandingCommand>
    {
        public override Task<IExecutionResult> ExecuteCommandAsync(
            LandingAggregate aggregate,
            LandingCommand command,
            CancellationToken cancellationToken)
        {
            var executionResult = aggregate.SetMagicNumer(command.Roverposition);
            return Task.FromResult(executionResult);
        }
    }
}
