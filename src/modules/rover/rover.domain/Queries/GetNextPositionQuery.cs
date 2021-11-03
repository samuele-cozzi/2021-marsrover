using EventFlow.Queries;
using rover.domain.Models;

namespace rover.domain.Queries
{
    public class GetNextPositionQuery : IQuery<PositionReadModel>
    {
        public Position Position { get; }
        public Moves Move { get; }
        public GetNextPositionQuery(Position position, Moves move)
        {
            Position = position;
            Move = move;
        }
    }
}
