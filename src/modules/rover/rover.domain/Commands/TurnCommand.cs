using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using rover.domain.Aggregates;
using rover.domain.Models;

namespace rover.domain.Commands
{
    public class TurnCommand : Command<TurnAggregate, TurnId, IExecutionResult>
    {
        public TurnCommand(TurnId aggregateId, FacingDirections facingDirection, Moves move) : base(aggregateId)
        {
            Move = move;
            FacingDirection = facingDirection;
        }

        public Moves Move { get; }
        public FacingDirections FacingDirection {  get;}
    }
}
