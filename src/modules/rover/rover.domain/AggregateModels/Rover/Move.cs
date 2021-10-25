using rover.domain.AggregateModels.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rover.domain.AggregateModels.Rover
{
    public class Move : Enumeration
    {
        public Move(int id, string name) : base(id, name)
        {
        }

        public static Move f = new(1, nameof(f));
        public static Move b = new(1, nameof(b));
        public static Move l = new(1, nameof(l));
        public static Move r = new(1, nameof(r));
    }
}
