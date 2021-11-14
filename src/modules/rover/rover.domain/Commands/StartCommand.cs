using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using rover.domain.Aggregates;
using rover.domain.Models;

namespace rover.domain.Commands
{
    public class StartCommand : Command<RoverPositionAggregate, RoverPositionAggregateId, IExecutionResult>
    {
        public StartCommand(RoverPositionAggregateId aggregateId, Moves[] move, bool stop) : base(aggregateId)
        {
            Move = move;
            Stop = stop;
        }

        public Moves[] Move { get; }
        public bool Stop { get; }
    }
}
