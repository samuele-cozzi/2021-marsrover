using EventFlow.Core;

namespace rover.domain.Models
{
    public class RoverId : Identity<RoverId>
    {
        public RoverId(string value) : base(value) { }
    }
}
