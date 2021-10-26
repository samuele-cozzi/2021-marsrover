using EventFlow.Queries;
using rover.domain.AggregateModels.Rover;
using System;
using System.Collections.Generic;
using System.Text;

namespace rover.application.Queries
{
    public class GetPositionByStartId : IQuery<Position>
    {
        public string StartId { get; }

        public GetPositionByStartId(string startId)
        {
            StartId = startId;
        }
    }
}
