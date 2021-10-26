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
    public class StartCommandHandler : CommandHandler<StartAggregate, StartId, IExecutionResult, StartCommand>
    {
        public override Task<IExecutionResult> ExecuteCommandAsync(
            StartAggregate aggregate,
            StartCommand command,
            CancellationToken cancellationToken)
        {
            var executionResult = aggregate.Move(command.Move, command.Stop);
            return Task.FromResult(executionResult);
        }
    }
}
