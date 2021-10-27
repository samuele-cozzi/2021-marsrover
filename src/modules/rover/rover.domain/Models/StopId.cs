using EventFlow.Core;

namespace rover.domain.Models
{
    public class StopId : Identity<StopId>
    {
        public StopId(string value) : base(value) { }
    }
}
