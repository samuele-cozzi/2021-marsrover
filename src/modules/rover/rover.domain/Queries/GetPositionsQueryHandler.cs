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
    public class GetPositionsQueryHandler : IQueryHandler<GetPositionsQuery, List<PositionReadModel>>
    {
        private IPositionRepository _positionRepository;

        public GetPositionsQueryHandler(
          IPositionRepository positionRepository)
        {
            _positionRepository = positionRepository;
        }

        public Task<List<PositionReadModel>> ExecuteQueryAsync(GetPositionsQuery query, CancellationToken cancellationToken)
        {
            return _positionRepository.GetPositionsAsync(cancellationToken);
        }
    }
}
