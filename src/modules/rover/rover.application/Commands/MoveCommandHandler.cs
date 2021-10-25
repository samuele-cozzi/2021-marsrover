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
    public class MoveCommandHandler : CommandHandler<MoveAggregate, RoverId, IExecutionResult, MoveCommand>
    {
        public override Task<IExecutionResult> ExecuteCommandAsync(
            MoveAggregate aggregate,
            MoveCommand command,
            CancellationToken cancellationToken)
        {
            var executionResult = aggregate.Move(command.Move);
            return Task.FromResult(executionResult);
        }
    }
}