using EventFlow.Core;
using EventFlow.ValueObjects;
using Newtonsoft.Json;

namespace rover.domain.Models
{
    [JsonConverter(typeof(SingleValueObjectConverter))] 
    public class MoveId : Identity<MoveId>
    {
        public MoveId(string value) : base(value) { }
    }
}
