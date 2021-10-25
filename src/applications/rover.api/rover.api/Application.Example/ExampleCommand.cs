using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;

namespace rover.api.Example
{
    public class ExampleCommand : Command<ExampleAggregate, ExampleId, IExecutionResult>
    {
        public ExampleCommand(
            ExampleId aggregateId,
            int magicNumber)
            : base(aggregateId)
        {
            MagicNumber = magicNumber;
        }

        public int MagicNumber { get; }
    }
}
