using System;
using EventFlow.Aggregates;
using EventFlow.ReadStores;
using rover.domain.Aggregates;
using rover.domain.DomainEvents;

namespace rover.domain.Models
{
    public class StopReadModel : IReadModel, IAmReadModelFor<StopAggregate, StopId, StoppedEvent>
    {
        public string AggregateId { get; private set; }
        public DateTimeOffset Timestamp { get; private set; }
        public int SequenceNumber { get; private set; }

        public FacingDirections FacingDirection { get; private set; }
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
        public bool IsBlocked { get; set; }


        public void Apply(IReadModelContext context, IDomainEvent<StopAggregate, StopId, StoppedEvent> domainEvent)
        {
            AggregateId = domainEvent.AggregateIdentity.Value;
            Timestamp = domainEvent.Timestamp;
            SequenceNumber = domainEvent.AggregateSequenceNumber;

            Latitude = domainEvent.AggregateEvent.Latitude;
            Longitude = domainEvent.AggregateEvent.Longitude;
            FacingDirection = domainEvent.AggregateEvent.FacingDirection;
            IsBlocked = domainEvent.AggregateEvent.IsBlocked;
        }

    }
}
