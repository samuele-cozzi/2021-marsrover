using rover.domain.AggregateModels.Rover;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rover.Settings
{
    public class RoverSettings
    {
        public Position Landing { get; set; }

        public int Distance { get; set; }
        public string ConnectionStringReadModel { get; set; }
    }
}
