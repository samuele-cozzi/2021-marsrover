using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using rover.application.Aggregates;
using rover.application.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace rover.application.Commands
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
