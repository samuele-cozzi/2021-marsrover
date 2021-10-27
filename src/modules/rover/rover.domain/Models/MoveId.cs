using EventFlow.Core;

namespace rover.domain.Models
{
    public class MoveId : Identity<MoveId>
    {
        public MoveId(string value) : base(value) { }
    }
}
