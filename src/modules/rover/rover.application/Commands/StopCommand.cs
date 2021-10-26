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
    public class StopCommand : Command<StopAggregate, StopId, IExecutionResult>
    {
        public StopCommand(StopId aggregateId, StartId startId, Position position, bool isBlocked, bool stop) : base(aggregateId)
        {
            RoverPosition = position;
            IsBlocked = isBlocked;
            Stop = stop;
            StartId = startId;
        }

        public Position RoverPosition { get; }
        public StartId StartId { get; set; }
        public bool IsBlocked { get; }
        public bool Stop { get; }
    }
}
