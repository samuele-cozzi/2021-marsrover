using EventFlow.Core;
using EventFlow.ValueObjects;
using Newtonsoft.Json;

namespace rover.domain.Models
{
    [JsonConverter(typeof(SingleValueObjectConverter))] 
    public class RoverId : Identity<RoverId>
    {
        public RoverId(string value) : base(value) { }
    }
}
