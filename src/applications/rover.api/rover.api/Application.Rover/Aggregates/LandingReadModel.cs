using EventFlow.Aggregates;
using EventFlow.ReadStores;
using rover.api.Application.Rover.Commands;
using rover.api.Application.Rover.DomainEvents;
using rover.api.Domain.Rover;

namespace rover.api.Application.Rover.Aggregates
{
    public class LandingReadModel : IReadModel, IAmReadModelFor<LandingAggregate, RoverId, LandedEvent>
    {
        public Position RoverPosition { get; private set; }

        public void Apply(
            IReadModelContext context,
            IDomainEvent<LandingAggregate, RoverId, LandedEvent> domainEvent)
        {
            RoverPosition = domainEvent.AggregateEvent.RoverPosition;
        }
    }
}
