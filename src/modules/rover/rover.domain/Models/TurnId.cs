using EventFlow.Core;
using EventFlow.ValueObjects;
using Newtonsoft.Json;

namespace rover.domain.Models
{
    [JsonConverter(typeof(SingleValueObjectConverter))] 
    public class TurnId : Identity<TurnId>
    {
        public TurnId(string value) : base(value) { }
    }
}
