using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace rover.domain.Queries
{
    public interface IPositionRepository
    {
        public Task<List<Models.PositionReadModel>> GetPositionsAsync(CancellationToken cancellationToken);
        public Task<Models.PositionReadModel> GetLastPositionsAsync(CancellationToken cancellationToken);
    }
}
