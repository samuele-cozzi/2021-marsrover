using EventFlow.Queries;
using rover.domain.Models;

namespace rover.domain.Queries
{
    public class GetLandingPositionQuery : IQuery<PositionReadModel>
    {
        public GetLandingPositionQuery()
        {
        }
    }
}