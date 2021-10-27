using EventFlow.Queries;
using rover.domain.AggregateModels.Rover;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace rover.application.Queries
{
    public class GetPositionByStartIdQueryHandler : IQueryHandler<GetPositionByStartId, Position>
    {
        private IReadModelRepository _readModelRepository;

        public GetPositionByStartIdQueryHandler(
          IUserReadModelRepository readModelRepository)
        {
            _readModelRepository = readModelRepository;
        }

        Task<Position> IQueryHandler<GetPositionByStartId, Position>
            .ExecuteQueryAsync(GetPositionByStartId query, CancellationToken cancellationToken)
        {
            return _readModelRepository.GetPositionByStartId(
              query.StartId,
              cancellationToken);
        }
    }
}
