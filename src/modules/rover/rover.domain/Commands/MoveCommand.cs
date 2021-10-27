using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using rover.domain.Aggregates;
using rover.domain.Models;

namespace rover.domain.Commands
{
    public  class MoveCommand : Command<MoveAggregate, MoveId, IExecutionResult>
    {
        public MoveCommand(MoveId aggregateId, Position position, Moves move) : base(aggregateId)
        {
            Move = move;
            Position = position;
        }

        public Moves Move { get; }
        public Position Position { get; }
    }
}
