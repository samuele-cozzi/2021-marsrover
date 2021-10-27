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
    public class PositionReadModel : IReadModel, IAmReadModelFor<PositionAggregate, PositionId, PositionChangedEvent>
    {
        public string AggregateId { get; private set; }
        public DateTimeOffset Timestamp { get; private set; }
        public int SequenceNumber { get; private set; }

        public FacingDirections FacingDirection { get; private set; }
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
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
