using EventFlow.Aggregates;
using EventFlow.ReadStores;
using rover.application.Aggregates;
using rover.application.DomainEvents;
using rover.domain.AggregateModels.Rover;
using System;
using System.Collections.Generic;
using System.Text;

namespace rover.application.Models
{
    public class TurnReadModel : IReadModel, IAmReadModelFor<TurnAggregate, TurnId, TurnedEvent>
    {
        public string AggregateId { get; private set; }
        public DateTimeOffset Timestamp { get; private set; }
        public int SequenceNumber { get; private set; }

        public FacingDirections FacingDirection { get; private set; }

        public void Apply(
            IReadModelContext context,
            IDomainEvent<TurnAggregate, TurnId, TurnedEvent> domainEvent)
        {
            AggregateId = domainEvent.AggregateIdentity.Value;
            Timestamp = domainEvent.Timestamp;
            SequenceNumber = domainEvent.AggregateSequenceNumber;

            FacingDirection = domainEvent.AggregateEvent.FacingDirection;
        }
    }
}
