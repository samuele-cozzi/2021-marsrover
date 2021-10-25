using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using rover.api.Application.Rover.Aggregates;
using rover.api.Domain.Rover;

namespace rover.api.Application.Rover.Commands
{
    public class LandingCommand : Command<LandingAggregate, RoverId, IExecutionResult>
    {
        public LandingCommand(
            RoverId aggregateId,
            Position position)
            : base(aggregateId)
        {
            Roverposition = position;
        }

        public Position Roverposition { get; }
    }
}
