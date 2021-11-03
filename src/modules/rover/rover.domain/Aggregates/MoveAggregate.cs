using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates;
using EventFlow.Aggregates.ExecutionResults;
using rover.domain.DomainEvents;
using rover.domain.Models;
using EventFlow.Queries;
using rover.domain.Queries;

namespace rover.domain.Aggregates
{
    public class MoveAggregate : AggregateRoot<MoveAggregate, MoveId>, IEmit<MovedEvent>
    {
        private readonly IQueryProcessor _queryProcessor;
        
        public MoveAggregate(
            MoveId id,
            IQueryProcessor queryProcessor
            ) : base(id) {
            _queryProcessor = queryProcessor;
        }

        public IExecutionResult Move(Position position, Moves move)
        {
            var newPosition = _queryProcessor.ProcessAsync(new GetNextPositionQuery(position, move), CancellationToken.None).Result;
                        
            if (!newPosition.IsBlocked)
            {
                Emit(new MovedEvent(newPosition.Latitude, newPosition.Longitude));

                return ExecutionResult.Success();
            }
            else
            {
                return ExecutionResult.Failed();
            }
        }

        public void Apply(MovedEvent aggregateEvent)
        {
            
        }
    }
}
