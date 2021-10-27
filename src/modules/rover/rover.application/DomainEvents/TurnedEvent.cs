using EventFlow.Aggregates;
using rover.application.Aggregates;
using rover.application.Models;
using rover.domain.AggregateModels.Rover;
using System;
using System.Collections.Generic;
using System.Text;

namespace rover.application.DomainEvents
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
