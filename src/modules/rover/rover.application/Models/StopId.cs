using EventFlow.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace rover.application.Models
{
    public class StopId : Identity<StopId>
    {
        public StopId(string value) : base(value) { }
    }
}
