using EventFlow.Aggregates;
using EventFlow.EventStores;
using rover.domain.Aggregates;
using rover.domain.Models;

namespace rover.domain.DomainEvents
{
    public class PositionChangedEvent : AggregateEvent<PositionAggregate, PositionId>
    {
        public FacingDirections FacingDirection { get; }
        public double Latitude { get; }
        public double Longitude { get; }
        public bool IsBlocked { get; }
        public string StartId { get; set; }
        public bool Stop { get; set; }

        public PositionChangedEvent(FacingDirections facingDirection, double latitude, double longitude, bool isBlocked, string startId, bool stop)
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
