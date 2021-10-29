using EventFlow.Queries;
using rover.domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace rover.domain.Queries
{
    public class GetLastPositionQuery : IQuery<PositionReadModel>
    {
        public GetLastPositionQuery()
        {
        }
    }
}
