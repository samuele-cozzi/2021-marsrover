using EventFlow.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace rover.application.Models
{
    public class TurnId : Identity<TurnId>
    {
        public TurnId(string value) : base(value) { }
    }
}
