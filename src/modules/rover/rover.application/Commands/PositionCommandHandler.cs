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
    public class PositionCommandHandler : CommandHandler<PositionAggregate, PositionId, IExecutionResult, PositionCommand>
    {
        public override Task<IExecutionResult> ExecuteCommandAsync(
            PositionAggregate aggregate,
            PositionCommand command,
            CancellationToken cancellationToken)
        {
            var executionResult = aggregate.ChangePosition(command.RoverPosition, command.IsBlocked, command.StartId, command.Stop);
            return Task.FromResult(executionResult);
        }
    }
}
