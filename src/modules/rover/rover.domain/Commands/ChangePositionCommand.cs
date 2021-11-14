using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using rover.domain.Aggregates;
using rover.domain.Models;

namespace rover.domain.Commands
{
    public class ChangePositionCommand : Command<RoverPositionAggregate, RoverPositionAggregateId, IExecutionResult>
    {
        public ChangePositionCommand(RoverPositionAggregateId aggregateId, Position position, bool isBlocked, bool stop) : base(aggregateId)
        {
            RoverPosition = position;
            IsBlocked = isBlocked;
            Stop = stop;
        }

        public Position RoverPosition { get; }
        public bool IsBlocked { get; }
        public bool Stop { get; set; }
    }
}
