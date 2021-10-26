using EventFlow.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace rover.application.Models
{
    public class StartId : Identity<StartId>
    {
        public StartId(string value) : base(value) { }
    }
}
