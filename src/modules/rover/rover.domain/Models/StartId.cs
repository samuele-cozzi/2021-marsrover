using EventFlow.Core;

namespace rover.domain.Models
{
    public class StartId : Identity<StartId>
    {
        public StartId(string value) : base(value) { }
    }
}
