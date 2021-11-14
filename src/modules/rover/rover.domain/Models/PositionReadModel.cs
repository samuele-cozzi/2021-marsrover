using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventFlow.Aggregates;
using EventFlow.ReadStores;
using rover.domain.Aggregates;
using rover.domain.DomainEvents;

namespace rover.domain.Models
{
    [Table("Position")]
    public class PositionReadModel : IReadModel, IAmReadModelFor<RoverPositionAggregate, RoverPositionAggregateId, PositionChangedEvent>
    {
        [Key]
        public string AggregateId { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public int SequenceNumber { get; set; }

        public FacingDirections FacingDirection { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool IsBlocked { get; set; }
        public string FacingDirectionName
        {
            get
            {
                return this.FacingDirection.ToString();
            }
        }


        public void Apply(IReadModelContext context, IDomainEvent<RoverPositionAggregate, RoverPositionAggregateId, PositionChangedEvent> domainEvent)
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
