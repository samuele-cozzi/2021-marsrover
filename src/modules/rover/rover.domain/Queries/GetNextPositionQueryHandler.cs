using EventFlow.Queries;
using rover.domain.Models;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.ReadStores.InMemory;
using Microsoft.Extensions.Options;
using rover.domain.Settings;

namespace rover.domain.Queries
{
    public class GetNextPositionQueryHandler : IQueryHandler<GetNextPositionQuery, PositionReadModel>
    {
        private readonly MarsSettings _options;
        private readonly IInMemoryReadStore<ObstacleReadModel> _readStore;
        private const int circle = 360;

        public GetNextPositionQueryHandler(
          IInMemoryReadStore<ObstacleReadModel> readStore,
          IOptions<MarsSettings> options)
        {
            _readStore = readStore;
        }

        public async Task<PositionReadModel> ExecuteQueryAsync(GetNextPositionQuery query, CancellationToken cancellationToken)
        {
            var obstacle = await _readStore
                .FindAsync(x => x.Latitude == query.Position.Coordinate.Latitude && x.Longitude == query.Position.Coordinate.Longitude, cancellationToken).ConfigureAwait(false);

            double x = query.Position.Coordinate.Longitude, y = query.Position.Coordinate.Latitude;
            var angularStep = circle / _options.AngularPartition;

            if (query.Move == Moves.f)
            {
                if (query.Position.FacingDirection == FacingDirections.N)
                    y += angularStep;
                if (query.Position.FacingDirection == FacingDirections.S)
                    y -= angularStep;
                if (query.Position.FacingDirection == FacingDirections.E)
                    x += angularStep;
                if (query.Position.FacingDirection == FacingDirections.W)
                    x -= angularStep;
            }

            if (query.Move == Moves.b)
            {
                if (query.Position.FacingDirection == FacingDirections.N)
                    y -= angularStep;
                if (query.Position.FacingDirection == FacingDirections.S)
                    y += angularStep;
                if (query.Position.FacingDirection == FacingDirections.E)
                    x -= angularStep;
                if (query.Position.FacingDirection == FacingDirections.W)
                    x += angularStep;
            }

            if (obstacle == null){
                return new PositionReadModel(){
                    FacingDirection = query.Position.FacingDirection,
                    Latitude = y,
                    Longitude = x,
                    IsBlocked = false
                };
            }
            else 
            {
                return new PositionReadModel(){
                    FacingDirection = query.Position.FacingDirection,                   
                    Latitude = query.Position.Coordinate.Latitude,
                    Longitude = query.Position.Coordinate.Longitude,
                    IsBlocked = true
                };
            }

            
        }
    }
}
