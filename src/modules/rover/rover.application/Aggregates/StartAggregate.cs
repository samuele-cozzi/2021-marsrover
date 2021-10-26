using EventFlow.Aggregates;
using EventFlow.Aggregates.ExecutionResults;
using rover.application.DomainEvents;
using rover.application.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace rover.application.Aggregates
{
    public class StartAggregate : AggregateRoot<StartAggregate, StartId>, IEmit<StartEvent>
    {
        private string[] move;

        public StartAggregate(StartId id) : base(id) { }

        public IExecutionResult Move(string[] move, bool stop)
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
