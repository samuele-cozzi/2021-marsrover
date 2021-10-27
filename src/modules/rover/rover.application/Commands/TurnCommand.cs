using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using rover.application.Aggregates;
using rover.application.Models;
using rover.domain.AggregateModels.Rover;
using System;
using System.Collections.Generic;
using System.Text;

namespace rover.application.Commands
{
    public class TurnCommand : Command<TurnAggregate, TurnId, IExecutionResult>
    {
        public TurnCommand(TurnId aggregateId, FacingDirections facingDirection, Moves move) : base(aggregateId)
        {
            Move = move;
            FacingDirection = facingDirection;
        }

        public Moves Move { get; }
        public FacingDirections FacingDirection {  get;}
    }
}
