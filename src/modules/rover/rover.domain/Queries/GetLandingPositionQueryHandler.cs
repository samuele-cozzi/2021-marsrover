using EventFlow.Queries;
using rover.domain.Models;
using rover.domain.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace rover.domain.Queries
{
    public class GetLandingPositionQueryHandler : IQueryHandler<GetLandingPositionQuery, LandingReadModel>
    {
        private IPositionRepository _positionRepository;

        public GetLandingPositionQueryHandler(
          IPositionRepository positionRepository)
        {
            _positionRepository = positionRepository;
        }

        public Task<LandingReadModel> ExecuteQueryAsync(GetLandingPositionQuery query, CancellationToken cancellationToken)
        {
            return _positionRepository.GetLandingPositionsAsync(cancellationToken);
        }
    }
}
