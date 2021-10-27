using EventFlow.Aggregates;
using rover.domain.Aggregates;
using rover.domain.Models;

namespace rover.domain.DomainEvents
{
    public class TurnedEvent : AggregateEvent<TurnAggregate, TurnId>
    {
        public FacingDirections FacingDirection { get; }


        public TurnedEvent(FacingDirections facingDirection)
        {
            this.FacingDirection = facingDirection;
        }
    }
}
