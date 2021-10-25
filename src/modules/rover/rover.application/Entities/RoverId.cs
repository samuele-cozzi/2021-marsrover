using EventFlow.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rover.application.Entities
{
    public class RoverId : Identity<RoverId>
    {
        public RoverId(string value) : base(value) { }
    }
}
