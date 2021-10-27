using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using rover.domain.Aggregates;
using rover.domain.Models;

namespace rover.domain.Commands
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
