using EventFlow.Aggregates;
using rover.domain.Aggregates;
using rover.domain.Models;

namespace rover.domain.DomainEvents
{
    public class ObstacleEvent : AggregateEvent<ObstacleAggregate, ObstacleId>
    {
        public double Latitude { get; }
        public double Longitude { get; }

        public ObstacleEvent(double latitude, double longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

    }
}
