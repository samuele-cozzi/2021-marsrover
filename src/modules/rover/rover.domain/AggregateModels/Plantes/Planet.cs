using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rover.domain.AggregateModels.Plantes
{
    public class Planet
    {
        public readonly int[,] Map;
        public Planet(int size)
        {
            Map = new int[size, size];
        }
    }
}
