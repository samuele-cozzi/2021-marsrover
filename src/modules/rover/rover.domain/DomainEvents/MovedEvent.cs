using EventFlow.Aggregates;
using rover.domain.Aggregates;
using rover.domain.Models;

namespace rover.domain.DomainEvents
{
    public class MovedEvent : AggregateEvent<MoveAggregate, MoveId>
    {
        public double Latitude { get; }
        public double Longitude { get; }
    

        public MovedEvent(double latitude, double longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }
    }

}
