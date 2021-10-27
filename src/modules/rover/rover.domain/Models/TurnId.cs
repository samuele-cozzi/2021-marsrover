using EventFlow.Core;

namespace rover.domain.Models
{
    public class TurnId : Identity<TurnId>
    {
        public TurnId(string value) : base(value) { }
    }
}
