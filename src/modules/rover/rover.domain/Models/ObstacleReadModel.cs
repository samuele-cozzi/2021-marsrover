using System;
using System.ComponentModel.DataAnnotations;
using EventFlow.Aggregates;
using EventFlow.ReadStores;
using rover.domain.Aggregates;
using rover.domain.DomainEvents;

namespace rover.domain.Models
{
    public class ObstacleReadModel : IReadModel, IAmReadModelFor<ObstacleAggregate, ObstacleId, ObstacleEvent>
    {
        [Key]
        public string AggregateId { get; private set; }
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }


        public void Apply(IReadModelContext context, IDomainEvent<ObstacleAggregate, ObstacleId, ObstacleEvent> domainEvent)
        {
            AggregateId = domainEvent.AggregateIdentity.Value;
            Latitude = domainEvent.AggregateEvent.Latitude;
            Longitude = domainEvent.AggregateEvent.Longitude;
        }

    }
}
