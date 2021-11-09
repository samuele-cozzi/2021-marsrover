using EventFlow.Queries;
using rover.domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace rover.domain.Queries
{
    public class GetLandingPositionQueryHandler : IQueryHandler<GetLandingPositionQuery, PositionReadModel>
    {
        private IPositionRepository _positionRepository;

        public GetLandingPositionQueryHandler(
          IPositionRepository positionRepository)
        {
            _positionRepository = positionRepository;
        }

        public Task<PositionReadModel> ExecuteQueryAsync(GetLandingPositionQuery query, CancellationToken cancellationToken)
        {
            return _positionRepository.GetLandingPositionsAsync(cancellationToken);
        }
    }
}
