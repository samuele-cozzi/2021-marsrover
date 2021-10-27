using EventFlow.Aggregates;
using EventFlow.Aggregates.ExecutionResults;
using rover.domain.DomainEvents;
using rover.domain.Models;

namespace rover.domain.Aggregates
{
    public class StartAggregate : AggregateRoot<StartAggregate, StartId>, IEmit<StartEvent>
    {
        private Moves[] move;

        public StartAggregate(StartId id) : base(id) { }

        public IExecutionResult Move(Moves[] move, bool stop)
        {
            Emit(new StartEvent(move, stop));

            return ExecutionResult.Success();
        }

        public void Apply(StartEvent aggregateEvent)
        {
            move = aggregateEvent.Move;
        }
    }
}
