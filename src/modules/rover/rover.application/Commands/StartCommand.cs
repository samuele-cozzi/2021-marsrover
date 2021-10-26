using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using rover.application.Aggregates;
using rover.application.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace rover.application.Commands
{
    public class StartCommand : Command<StartAggregate, StartId, IExecutionResult>
    {
        public StartCommand(StartId aggregateId, string[] move, bool stop) : base(aggregateId)
        {
            Move = move;
            Stop = stop;
        }

        public string[] Move { get; }
        public bool Stop { get; }
    }
}
