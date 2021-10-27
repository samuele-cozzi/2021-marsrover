using EventFlow.Aggregates;
using rover.application.Aggregates;
using rover.application.Entities;
using rover.application.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace rover.application.DomainEvents
{
    public class MoveEvent : AggregateEvent<MoveAggregate, MoveId>
    {
        public string Move { get; }

        public MoveEvent(string move)
        {
            this.Move = move;
        }

    }
}
