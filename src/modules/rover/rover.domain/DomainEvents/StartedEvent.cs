using EventFlow.Aggregates;
using EventFlow.EventStores;
using rover.domain.Aggregates;
using rover.domain.Models;

namespace rover.domain.DomainEvents
{
    [EventVersion("started", 1)]
    public class StartedEvent : AggregateEvent<RoverPositionAggregate, RoverPositionAggregateId>
    {
        public Moves[] Move { get; }
        public bool Stop { get; set; }

        public StartedEvent(Moves[] move, bool stop)
        {
            this.Move = move;
            this.Stop = stop;
        }

    }
}
