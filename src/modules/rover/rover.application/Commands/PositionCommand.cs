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
    public class PositionCommand : Command<PositionAggregate, PositionId, IExecutionResult>
    {
        public PositionCommand(PositionId aggregateId, Position position, bool isBlocked, string startId, bool stop) : base(aggregateId)
        {
            RoverPosition = position;
            IsBlocked = isBlocked;
            StartId = startId;
            Stop = stop;
        }

        public Position RoverPosition { get; }
        public bool IsBlocked { get; }
        public string StartId { get; }
        public bool Stop { get; set; }
    }
}
