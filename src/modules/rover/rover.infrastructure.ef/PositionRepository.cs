using Microsoft.EntityFrameworkCore;
using rover.domain.Models;
using rover.domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace rover.infrastructure.ef
{
    public class PositionRepository : IPositionRepository, IDisposable
    {
        private DBContextControlRoom _context;

        public PositionRepository(DBContextControlRoom context)
        {
            _context = context;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Task<PositionReadModel> GetLastPositionsAsync(CancellationToken cancellationToken)
        {
            return _context.Positions.OrderByDescending(p => p.Timestamp).FirstOrDefaultAsync();
        }

        public Task<PositionReadModel> GetLandingPositionsAsync(CancellationToken cancellationToken)
        {
            return _context.Positions.OrderBy(p => p.Timestamp).FirstOrDefaultAsync();
        }

        public Task<List<PositionReadModel>> GetPositionsAsync(CancellationToken cancellationToken)
        {
            return _context.Positions.ToListAsync();
        }
    }
}
