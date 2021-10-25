using EventFlow.Core;

namespace rover.api.Application.Rover.Commands
{
    public class RoverId : Identity<RoverId>
    {
        public RoverId(string value) : base(value) { }
    }
}
