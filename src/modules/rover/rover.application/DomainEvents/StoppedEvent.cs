using EventFlow.Aggregates;
using rover.application.Aggregates;
using rover.application.Models;
using rover.domain.AggregateModels.Rover;
using System;
using System.Collections.Generic;
using System.Text;

namespace rover.application.DomainEvents
{
    public class StoppedEvent : AggregateEvent<StopAggregate, StopId>
    {
        public string StartId { get; set; }
        public FacingDirections FacingDirection { get; }
        public double Latitude { get; }
        public double Longitude { get; }
        public bool IsBlocked { get; }
        public bool Stop { get; set; }

        public StoppedEvent(string startId, FacingDirections facingDirection, double latitude, double longitude, bool isBlocked, bool stop)
        {
            this.StartId = startId;
            this.FacingDirection = facingDirection;
            this.Latitude = latitude;
            this.Longitude = longitude;
            this.IsBlocked = isBlocked;
            this.Stop = stop;
        }

    }
}
