using EventFlow.Aggregates;
using EventFlow.EventStores;
using rover.domain.Aggregates;
using rover.domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rover.domain.DomainEvents
{
    [EventVersion("landing", 1)]
    public class LandedEvent : AggregateEvent<RoverAggregate, RoverAggregateId>
    {
        public FacingDirections FacingDirection { get; }
        public double Latitude { get; }
        public double Longitude { get; }

        public LandedEvent(FacingDirections facingDirection, double latitude, double longitude)
        {
            this.FacingDirection = facingDirection;
            this.Latitude = latitude;
            this.Longitude = longitude;
        }
    }
}
