using EventFlow.Aggregates;
using EventFlow.Aggregates.ExecutionResults;
using rover.application.DomainEvents;
using rover.application.Entities;
using rover.domain.AggregateModels.Rover;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rover.application.Aggregates
{
    public class MoveAggregate : AggregateRoot<MoveAggregate, RoverId>, IEmit<MoveEvent>
    {
        private string[] move;

        public MoveAggregate(RoverId id) : base(id) { }

        public IExecutionResult Move(string[] move)
        {
            Emit(new MoveEvent(move));

            return ExecutionResult.Success();
        }

        public void Apply(MoveEvent aggregateEvent)
        {
            move = aggregateEvent.Move;
        }
    }
}
