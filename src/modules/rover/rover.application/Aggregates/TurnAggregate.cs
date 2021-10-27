using EventFlow.Aggregates;
using EventFlow.Aggregates.ExecutionResults;
using rover.application.DomainEvents;
using rover.application.Models;
using rover.domain.AggregateModels.Rover;
using rover.domain.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace rover.application.Aggregates
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
