using EventFlow.Core;

namespace rover.api.Example
{
    public class ExampleId : Identity<ExampleId>
    {
        public ExampleId(string value) : base(value) { }
    }
}
