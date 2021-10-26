using EventFlow.Aggregates;
using EventFlow.ReadStores;
using rover.application.Aggregates;
using rover.application.DomainEvents;
using rover.application.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace rover.application.Models
{
    public class StopReadModel : IReadModel, IAmReadModelFor<StopAggregate, StopId, StoppedEvent>
    {
        public string AggregateId { get; private set; }
        public DateTimeOffset Timestamp { get; private set; }
        public int SequenceNumber { get; private set; }

        public string FacingDirection { get; private set; }
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
