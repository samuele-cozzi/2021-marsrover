using EventFlow.Aggregates;
using EventFlow.EventStores;
using EventFlow.ReadStores;
using rover.domain.Aggregates;
using rover.domain.DomainEvents;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rover.domain.Models
{
    [Table("Landing")]
    public class LandingReadModel : IReadModel, IAmReadModelFor<RoverAggregate, RoverAggregateId, LandedEvent>
    {
        [Key]
        public string AggregateId { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public int SequenceNumber { get; set; }

        public FacingDirections FacingDirection { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string FacingDirectionName
        {
            get
            {
                return this.FacingDirection.ToString();
            }
        }


        public void Apply(IReadModelContext context, IDomainEvent<RoverAggregate, RoverAggregateId, LandedEvent> domainEvent)
        {
            AggregateId = domainEvent.AggregateIdentity.Value;
            Timestamp = domainEvent.Timestamp;
            SequenceNumber = domainEvent.AggregateSequenceNumber;

            Latitude = domainEvent.AggregateEvent.Latitude;
            Longitude = domainEvent.AggregateEvent.Longitude;
            FacingDirection = domainEvent.AggregateEvent.FacingDirection;
        }
    }
}
