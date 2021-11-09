using EventFlow.Core;
using EventFlow.ValueObjects;
using Newtonsoft.Json;

namespace rover.domain.Models
{
    [JsonConverter(typeof(SingleValueObjectConverter))] 
    public class PositionId : Identity<PositionId>
    {
        public PositionId(string value) : base(value) { }
    }
}
