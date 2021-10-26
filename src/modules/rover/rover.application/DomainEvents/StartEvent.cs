using EventFlow.Aggregates;
using rover.application.Aggregates;
using rover.application.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace rover.application.DomainEvents
{
    public class StartEvent : AggregateEvent<StartAggregate, StartId>
    {
        public string[] Move { get; }
        public bool Stop { get; set; }

        public StartEvent(string[] move, bool stop)
        {
            this.Move = move;
            this.Stop = stop;
        }

    }
}
