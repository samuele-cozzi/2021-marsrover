using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using rover.domain.Aggregates;
using rover.domain.Models;

namespace rover.domain.Commands
{
    public class StartCommand : Command<StartAggregate, StartId, IExecutionResult>
    {
        public StartCommand(StartId aggregateId, Moves[] move, bool stop) : base(aggregateId)
        {
            Move = move;
            Stop = stop;
        }

        public Moves[] Move { get; }
        public bool Stop { get; }
    }
}
