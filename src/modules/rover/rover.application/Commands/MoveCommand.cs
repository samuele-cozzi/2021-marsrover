using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using rover.application.Aggregates;
using rover.application.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rover.application.Commands
{
    public  class MoveCommand : Command<MoveAggregate, RoverId, IExecutionResult>
    {
        public MoveCommand(RoverId aggregateId, string[] move) : base(aggregateId)
        {
            Move = move;
        }

        public string[] Move { get; }
    }
}
