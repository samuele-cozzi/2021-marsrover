using EventFlow.Core;

namespace rover.domain.Models
{
    public class ObstacleId : Identity<ObstacleId>
    {
        public ObstacleId(string value) : base(value) { }
    }
}