using EventFlow.Aggregates;
using EventFlow.Aggregates.ExecutionResults;
using rover.domain.DomainEvents;
using rover.domain.Models;
using rover.domain.Utilities;

namespace rover.domain.Aggregates
{
    public class TurnAggregate : AggregateRoot<TurnAggregate, TurnId>, IEmit<TurnedEvent>
    {
        public TurnAggregate(TurnId id) : base(id) { }

        public IExecutionResult Turn(FacingDirections facingDirection, Moves move)
        {
            if (move == Moves.r)
            {
                facingDirection = facingDirection.Next(); 
            }

            if (move == Moves.l)
            {
                facingDirection = facingDirection.Previous();
            }

            Emit(new TurnedEvent(facingDirection));

            return ExecutionResult.Success();
        }

        public void Apply(TurnedEvent aggregateEvent)
        {
            
        }
    }
}
