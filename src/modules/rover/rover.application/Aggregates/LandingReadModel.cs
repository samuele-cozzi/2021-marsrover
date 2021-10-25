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
    public class LandingReadModel : IReadModel, IAmReadModelFor<LandingAggregate, RoverId, LandedEvent>
    {
        public string FacingDirection { get; private set; }
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
        public DateTimeOffset Timestamp { get; private set; }
        public int SequenceNumber { get; private set;}
        public string AggregateId { get; private set; }

        public void Apply(IReadModelContext context, IDomainEvent<LandingAggregate, RoverId, LandedEvent> domainEvent)
        {

            Latitude = domainEvent.AggregateEvent.Latitude;
            Longitude = domainEvent.AggregateEvent.Longitude;
            FacingDirection = domainEvent.AggregateEvent.FacingDirection;
            AggregateId = domainEvent.AggregateIdentity.Value;
            Timestamp = domainEvent.Timestamp;
            SequenceNumber = domainEvent.AggregateSequenceNumber;
        }

    }
}
