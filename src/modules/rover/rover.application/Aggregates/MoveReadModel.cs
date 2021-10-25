using EventFlow.Aggregates;
using EventFlow.ReadStores;
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
    public class MoveReadModel : IReadModel, IAmReadModelFor<MoveAggregate, RoverId, MovedEvent>
    {
        public Position position { get; private set; }

        public void Apply(
            IReadModelContext context,
            IDomainEvent<MoveAggregate, RoverId, MovedEvent> domainEvent)
        {
            position = new Position()
            {
                Latitude = domainEvent.AggregateEvent.Latitude,
                Longitude = domainEvent.AggregateEvent.Longitude,
                FacingDirection = domainEvent.AggregateEvent.FacingDirection
            };
        }
    }
}
