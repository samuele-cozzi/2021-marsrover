using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using rover.application.Aggregates;
using rover.application.Models;
using rover.domain.AggregateModels.Rover;
using System;
using System.Collections.Generic;
using System.Text;

namespace rover.application.Commands
{
    public class StartCommand : Command<StartAggregate, StartId, IExecutionResult>
    {
        public StartCommand(StartId aggregateId, Moves[] move, bool stop) : base(aggregateId)
        {
            Move = move;
            Stop = stop;
        }

        public Moves[] Move { get; }
        public bool Stop { get; }
    }
}
