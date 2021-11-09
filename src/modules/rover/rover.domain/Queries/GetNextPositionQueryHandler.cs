using EventFlow.Queries;
using rover.domain.Models;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.ReadStores.InMemory;
using Microsoft.Extensions.Options;
using rover.domain.Settings;
using System;

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
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options)); ;
        }

        public async Task<PositionReadModel> ExecuteQueryAsync(GetNextPositionQuery query, CancellationToken cancellationToken)
        {
            double longitude = query.Position.Coordinate.Longitude, latitude = query.Position.Coordinate.Latitude;
            var angularStep = circle / _options.AngularPartition;

            if (query.Move == Moves.f)
            {
                if (query.Position.FacingDirection == FacingDirections.N)
                    latitude += angularStep;
                if (query.Position.FacingDirection == FacingDirections.S)
                    latitude -= angularStep;
                if (query.Position.FacingDirection == FacingDirections.E)
                    longitude += angularStep;
                if (query.Position.FacingDirection == FacingDirections.W)
                    longitude -= angularStep;
            }

            if (query.Move == Moves.b)
            {
                if (query.Position.FacingDirection == FacingDirections.N)
                    latitude -= angularStep;
                if (query.Position.FacingDirection == FacingDirections.S)
                    latitude += angularStep;
                if (query.Position.FacingDirection == FacingDirections.E)
                    longitude -= angularStep;
                if (query.Position.FacingDirection == FacingDirections.W)
                    longitude += angularStep;
            }

            //boudaries
            longitude = (longitude >= -circle/2 && longitude <= circle/2) ?  longitude : Math.Sign(longitude) * (2 * angularStep) - longitude;
            latitude = (latitude >= -circle/4 && latitude <= circle/4) ? latitude : throw new Exception();

            var obstacle = await _readStore
                .FindAsync(x => x.Latitude == latitude && x.Longitude == longitude, cancellationToken).ConfigureAwait(false);

            if (obstacle?.Count == 0){
                return new PositionReadModel(){
                    FacingDirection = query.Position.FacingDirection,
                    Latitude = latitude,
                    Longitude = longitude,
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
