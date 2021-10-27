//using rover.domain.AggregateModels.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rover.domain.AggregateModels.Rover
{
    public class Position
    {
        public double Latitude {  get; set; }
        public double Longitude { get; set; }
        public FacingDirections FacingDirection { get; set; }
    }
}
