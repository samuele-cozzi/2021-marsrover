using EventFlow.Core;
using EventFlow.ValueObjects;
using Newtonsoft.Json;

namespace rover.domain.Models
{
    [JsonConverter(typeof(SingleValueObjectConverter))] 
    public class ObstacleId : Identity<ObstacleId>
    {
        public ObstacleId(string value) : base(value) { }
    }
}