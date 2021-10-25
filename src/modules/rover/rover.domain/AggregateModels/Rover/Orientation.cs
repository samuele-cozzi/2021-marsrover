using rover.domain.AggregateModels.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rover.domain.AggregateModels.Rover
{
    public class Orientation : Enumeration
    {
        public Orientation(int id, string name): base(id, name)
        {
        }

        public static Orientation N = new(1, nameof(N));
        public static Orientation S = new(2, nameof(S));
        public static Orientation W = new(3, nameof(W));
        public static Orientation E = new(4, nameof(E));
    }
}
