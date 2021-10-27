using System;
using EventFlow.Aggregates;
using EventFlow.ReadStores;
using rover.domain.Aggregates;
using rover.domain.DomainEvents;

namespace rover.domain.Models
{
    public class MoveReadModel : IReadModel, IAmReadModelFor<MoveAggregate, MoveId, MovedEvent>
    {
        public string AggregateId { get; private set; }
        public DateTimeOffset Timestamp { get; private set; }
        public int SequenceNumber { get; private set; }

        public double Latitude { get; private set; }
        public double Longitude { get; private set; }

        public void Apply(
            IReadModelContext context,
            IDomainEvent<MoveAggregate, MoveId, MovedEvent> domainEvent)
        {
            AggregateId = domainEvent.AggregateIdentity.Value;
            Timestamp = domainEvent.Timestamp;
            SequenceNumber = domainEvent.AggregateSequenceNumber;

            Latitude = domainEvent.AggregateEvent.Latitude;
            Longitude = domainEvent.AggregateEvent.Longitude;
        }
    }
}
