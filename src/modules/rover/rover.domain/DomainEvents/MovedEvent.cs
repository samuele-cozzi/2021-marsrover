using EventFlow.Aggregates;
using EventFlow.EventStores;
using rover.domain.Aggregates;
using rover.domain.Models;

namespace rover.domain.DomainEvents
{
    [EventVersion("stopped", 1)]
    public class MovedEvent : AggregateEvent<RoverAggregate, RoverAggregateId>
    {
        public FacingDirections FacingDirection { get; }
        public double Latitude { get; }
        public double Longitude { get; }
        public double CoordinatePrecision { get; }
        public bool IsBlocked { get; }
        public bool Stop { get; set; }

        public MovedEvent(FacingDirections facingDirection, double latitude, double longitude, double coordinatePrecision, bool isBlocked, bool stop)
        {
            this.FacingDirection = facingDirection;
            this.Latitude = latitude;
            this.Longitude = longitude;
            this.IsBlocked = isBlocked;
            this.Stop = stop;
            this.CoordinatePrecision = coordinatePrecision;
        }

    }
}
