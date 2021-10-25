using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rover.domain.AggregateModels.Rover
{
    public class MoveResponse
    {
        public object[] Move { get; set; }
        public Position StartPosition { get; set; }
        public Position StopPosition { get; set; }
    }
}
