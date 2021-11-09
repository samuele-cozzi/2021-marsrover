using EventFlow.Core;
using EventFlow.ValueObjects;
using Newtonsoft.Json;

namespace rover.domain.Models
{
    [JsonConverter(typeof(SingleValueObjectConverter))] 
    public class StopId : Identity<StopId>
    {
        public StopId(string value) : base(value) { }
    }
}
