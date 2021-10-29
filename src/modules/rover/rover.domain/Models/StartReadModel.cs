using System;
using System.ComponentModel.DataAnnotations;
using EventFlow.Aggregates;
using EventFlow.ReadStores;
using rover.domain.Aggregates;
using rover.domain.DomainEvents;

namespace rover.domain.Models
{
    public class StartReadModel : IReadModel, IAmReadModelFor<StartAggregate, StartId, StartEvent>
    {
        [Key]
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

            Move = string.Join<Moves>("-", domainEvent.AggregateEvent.Move);
        }
    }
}
