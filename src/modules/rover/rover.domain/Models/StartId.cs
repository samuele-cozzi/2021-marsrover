using EventFlow.Core;
using EventFlow.ValueObjects;
using Newtonsoft.Json;

namespace rover.domain.Models
{
    [JsonConverter(typeof(SingleValueObjectConverter))] 
    public class StartId : Identity<StartId>
    {
        public StartId(string value) : base(value) { }
    }
}
