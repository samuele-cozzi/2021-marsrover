using EventFlow.Aggregates;
using rover.application.Aggregates;
using rover.application.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace rover.application.DomainEvents
{
    public class MoveEvent : AggregateEvent<MoveAggregate, RoverId>
    {
        public string[] Move { get; }

        public MoveEvent(string[] move)
        {
            this.Move = move;
        }

    }
}
