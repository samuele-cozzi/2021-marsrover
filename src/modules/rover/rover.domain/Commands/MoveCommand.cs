using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using rover.domain.Aggregates;
using rover.domain.Models;

namespace rover.domain.Commands
{
    public  class MoveCommand : Command<RoverAggregate, RoverAggregateId, IExecutionResult>
    {
        public MoveCommand(RoverAggregateId aggregateId, Moves[] moves, bool isBlocked) : base(aggregateId)
        {
            Moves = moves;
            IsBlocked = isBlocked;
        }

        public Moves[] Moves { get; }
        public bool IsBlocked { get; }
    }
}
