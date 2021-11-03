using System;
using System.ComponentModel.DataAnnotations;
using EventFlow.Aggregates;
using EventFlow.ReadStores;
using rover.domain.Aggregates;
using rover.domain.DomainEvents;

namespace rover.domain.Models
{
    public class PositionReadModel : IReadModel, IAmReadModelFor<PositionAggregate, PositionId, PositionChangedEvent>
    {
        [Key]
        public string AggregateId { get; private set; }
        public DateTimeOffset Timestamp { get; private set; }
        public int SequenceNumber { get; private set; }

        public FacingDirections FacingDirection { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool IsBlocked { get; set; }
        public string StartId { get; set; }


        public void Apply(IReadModelContext context, IDomainEvent<PositionAggregate, PositionId, PositionChangedEvent> domainEvent)
        {
            AggregateId = domainEvent.AggregateIdentity.Value;
            Timestamp = domainEvent.Timestamp;
            SequenceNumber = domainEvent.AggregateSequenceNumber;

            Latitude = domainEvent.AggregateEvent.Latitude;
            Longitude = domainEvent.AggregateEvent.Longitude;
            FacingDirection = domainEvent.AggregateEvent.FacingDirection;
            IsBlocked = domainEvent.AggregateEvent.IsBlocked;
            StartId = domainEvent.AggregateEvent.StartId;
        }

    }
}
