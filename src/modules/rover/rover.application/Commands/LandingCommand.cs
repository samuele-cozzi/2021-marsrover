using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using rover.application.Aggregates;
using rover.application.Entities;
using rover.domain.AggregateModels.Rover;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rover.application.Commands
{
    public class LandingCommand : Command<LandingAggregate, RoverId, IExecutionResult>
    {
        public LandingCommand(RoverId aggregateId, Position position) : base(aggregateId)
        {
            RoverPosition = position;
        }

        public Position RoverPosition { get; }
    }
}
