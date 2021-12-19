using EventFlow.EntityFramework;
using Microsoft.EntityFrameworkCore;
using rover.domain.Models;
using rover.domain.Queries;
using rover.domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace rover.infrastructure.ef
{
    public class PositionRepository : IPositionRepository
    {
        private readonly IDbContextProvider<DBContextControlRoom> _contextProvider;

        public PositionRepository(IDbContextProvider<DBContextControlRoom> contextProvider)
        {
            _contextProvider = contextProvider;
        }

        public async Task<PositionReadModel> GetLastPositionsAsync(CancellationToken cancellationToken)
        {
            using (var context = _contextProvider.CreateContext())
            {
                var result = await context.Positions
                    .OrderByDescending(x => x.SequenceNumber)
                    .FirstOrDefaultAsync();
                return result;
            }
        }

        public async Task<LandingReadModel> GetLandingPositionsAsync(CancellationToken cancellationToken)
        {
            using (var context = _contextProvider.CreateContext())
            {
                var result = await context.Landing
                    .OrderByDescending(x => x.SequenceNumber)
                    .FirstOrDefaultAsync();
                return result;
            }
        }

        public async Task<List<PositionReadModel>> GetPositionsAsync(CancellationToken cancellationToken)
        {
            using (var context = _contextProvider.CreateContext())
            {
                var result = await context.Positions.ToListAsync();
                return result;
            }
        }
    }
}
