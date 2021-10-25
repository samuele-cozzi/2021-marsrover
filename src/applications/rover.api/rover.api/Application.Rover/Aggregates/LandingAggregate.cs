using EventFlow.Aggregates;
using EventFlow.Aggregates.ExecutionResults;
using rover.api.Application.Rover.Commands;
using rover.api.Application.Rover.DomainEvents;
using rover.api.Domain.Rover;

namespace rover.api.Application.Rover.Aggregates
{
    public class LandingAggregate : AggregateRoot<LandingAggregate, RoverId>, IEmit<LandedEvent>
    {

        private Position position;

        public LandingAggregate(RoverId id) : base(id) { }

        // Method invoked by our command
        public IExecutionResult SetMagicNumer(Position position)
        {
            //if (_magicNumber.HasValue)
            //    return ExecutionResult.Failed("Magic number already set");

            Emit(new LandedEvent(position));

            return ExecutionResult.Success();
        }

        // We apply the event as part of the event sourcing system. EventFlow
        // provides several different methods for doing this, e.g. state objects,
        // the Apply method is merely the simplest
        public void Apply(LandedEvent aggregateEvent)
        {
            position = aggregateEvent.RoverPosition;
        }
    }
}
