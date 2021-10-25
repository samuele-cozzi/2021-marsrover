using EventFlow.Aggregates;
using rover.api.Application.Rover.Aggregates;
using rover.api.Application.Rover.Commands;
using rover.api.Domain.Rover;

namespace rover.api.Application.Rover.DomainEvents
{
    public class LandedEvent : AggregateEvent<LandingAggregate, RoverId>
    {
        public LandedEvent(Position position)
        {
            RoverPosition = position;
        }

        public Position RoverPosition { get; }
    }
}
