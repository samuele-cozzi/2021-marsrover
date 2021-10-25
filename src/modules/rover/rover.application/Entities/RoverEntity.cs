using EventFlow.Entities;
using rover.domain.AggregateModels.Rover;
using System;
using System.Collections.Generic;
using System.Text;

namespace rover.application.Entities
{
    public class RoverEntity : Entity<RoverId>
    {
        public RoverEntity(RoverId id) : base(id)
        {
        }

        public Position Position { get; set; }
    }
}
