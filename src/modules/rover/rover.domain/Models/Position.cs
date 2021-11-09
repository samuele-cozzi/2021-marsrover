//using rover.domain.AggregateModels.Abstracts;

namespace rover.domain.Models
{
    public class Position
    {
        public Coordinate Coordinate { get; set; }
        public FacingDirections FacingDirection { get; set; }
    }
}
