using EventFlow.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace rover.application.Models
{
    public class MoveId : Identity<MoveId>
    {
        public MoveId(string value) : base(value) { }
    }
}
