//using rover.domain.AggregateModels.Abstracts;

namespace rover.domain.Models
{
    public class Position
    {
        public double Latitude {  get; set; }
        public double Longitude { get; set; }
        public FacingDirections FacingDirection { get; set; }
    }
}
