using EventFlow.Aggregates;
using EventFlow.EventStores;
using rover.application.Aggregates;
using rover.application.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace rover.application.DomainEvents
{
    [EventVersion("PositionChangedEvent", 1)]
    public class PositionChangedEvent : AggregateEvent<PositionAggregate, PositionId>
    {
        public string FacingDirection { get; }
        public double Latitude { get; }
        public double Longitude { get; }
        public bool IsBlocked { get; }
        public string StartId { get; set; }
        public bool Stop { get; set; }

        public PositionChangedEvent(string facingDirection, double latitude, double longitude, bool isBlocked, string startId, bool stop)
        {
            this.FacingDirection = facingDirection;
            this.Latitude = latitude;
            this.Longitude = longitude;
            this.IsBlocked = isBlocked;
            this.StartId = startId;
            this.Stop = stop;
        }

    }
}
