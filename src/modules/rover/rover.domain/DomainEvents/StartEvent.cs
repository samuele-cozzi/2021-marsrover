using EventFlow.Aggregates;
using rover.domain.Aggregates;
using rover.domain.Models;

namespace rover.domain.DomainEvents
{
    public class StartEvent : AggregateEvent<StartAggregate, StartId>
    {
        public Moves[] Move { get; }
        public bool Stop { get; set; }

        public StartEvent(Moves[] move, bool stop)
        {
            this.Move = move;
            this.Stop = stop;
        }

    }
}
