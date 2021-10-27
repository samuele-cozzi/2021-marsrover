using EventFlow.Aggregates;
using EventFlow.ReadStores;
using rover.application.Aggregates;
using rover.application.DomainEvents;
using rover.application.Entities;
using rover.domain.AggregateModels.Rover;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rover.application.Models
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
