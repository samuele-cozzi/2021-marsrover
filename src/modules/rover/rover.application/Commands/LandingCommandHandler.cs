using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using rover.application.Aggregates;
using rover.application.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace rover.application.Commands
{
    public class LandingCommandHandler : CommandHandler<LandingAggregate, RoverId, IExecutionResult, LandingCommand>
    {
        public override Task<IExecutionResult> ExecuteCommandAsync(
            LandingAggregate aggregate,
            LandingCommand command,
            CancellationToken cancellationToken)
        {
            var executionResult = aggregate.Land(command.RoverPosition);
            return Task.FromResult(executionResult);
        }
    }
}
