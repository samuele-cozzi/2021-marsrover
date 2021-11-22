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
    public class GetLastPositionQueryHandler : IQueryHandler<GetLastPositionQuery, PositionReadModel>
    {
        private IPositionRepository _positionRepository;

        public GetLastPositionQueryHandler(
          IPositionRepository positionRepository)
        {
            _positionRepository = positionRepository;
        }

        public Task<PositionReadModel> ExecuteQueryAsync(GetLastPositionQuery query, CancellationToken cancellationToken)
        {
            return _positionRepository.GetLastPositionsAsync(cancellationToken);
        }
    }
}
