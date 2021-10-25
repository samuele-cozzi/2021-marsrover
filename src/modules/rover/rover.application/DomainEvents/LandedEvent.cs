using EventFlow.Aggregates;
using rover.application.Aggregates;
using rover.application.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rover.application.DomainEvents
{
    public class LandedEvent : AggregateEvent<LandingAggregate, RoverId>
    {
        public string FacingDirection { get;  }
        public double Latitude { get; } 
        public double Longitude { get; }

        public LandedEvent(string facingDirection, double latitude, double longitude)
        {
            this.FacingDirection = facingDirection;
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

    }
}
