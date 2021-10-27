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
