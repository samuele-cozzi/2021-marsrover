using EventFlow.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace rover.application.Models
{
    public class PositionId : Identity<PositionId>
    {
        public PositionId(string value) : base(value) { }
    }
}
