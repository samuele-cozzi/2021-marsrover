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
    public class MoveReadModel : IReadModel, IAmReadModelFor<MoveAggregate, RoverId, MoveEvent>
    {
        public string AggregateId { get; private set; }
        public DateTimeOffset Timestamp { get; private set; }
        public int SequenceNumber { get; private set; }

        public string Move { get; set; }

        public void Apply(
            IReadModelContext context,
            IDomainEvent<MoveAggregate, RoverId, MoveEvent> domainEvent)
        {
            AggregateId = domainEvent.AggregateIdentity.Value;
            Timestamp = domainEvent.Timestamp;
            SequenceNumber = domainEvent.AggregateSequenceNumber;

            Move = string.Join("-", domainEvent.AggregateEvent.Move);
        }
    }
}
