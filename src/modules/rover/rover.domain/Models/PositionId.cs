using EventFlow.Core;

namespace rover.domain.Models
{
    public class PositionId : Identity<PositionId>
    {
        public PositionId(string value) : base(value) { }
    }
}
