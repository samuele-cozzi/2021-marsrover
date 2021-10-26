using EventFlow.Aggregates;
using EventFlow.ReadStores;
using rover.application.Aggregates;
using rover.application.DomainEvents;
using System;
using System.Collections.Generic;
using System.Text;

namespace rover.application.Models
{
    public class StartReadModel : IReadModel, IAmReadModelFor<StartAggregate, StartId, StartEvent>
    {
        public string AggregateId { get; private set; }
        public DateTimeOffset Timestamp { get; private set; }
        public int SequenceNumber { get; private set; }

        public string Move { get; set; }

        public void Apply(
            IReadModelContext context,
            IDomainEvent<StartAggregate, StartId, StartEvent> domainEvent)
        {
            AggregateId = domainEvent.AggregateIdentity.Value;
            Timestamp = domainEvent.Timestamp;
            SequenceNumber = domainEvent.AggregateSequenceNumber;

            Move = string.Join("-", domainEvent.AggregateEvent.Move);
        }
    }
}
