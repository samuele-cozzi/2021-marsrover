using EventFlow.Aggregates;
using EventFlow.Aggregates.ExecutionResults;
using rover.application.DomainEvents;
using rover.application.Entities;
using rover.domain.AggregateModels.Rover;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rover.application.Aggregates
{
    public class MoveAggregate : AggregateRoot<MoveAggregate, RoverId>, IEmit<MovedEvent>
    {
        private Position position;

        public MoveAggregate(RoverId id) : base(id) { }

        public IExecutionResult Move(string[] move)
        {
            //MoveResponse response = new MoveResponse();
            position = new Position()
            {
                FacingDirection = "N",
                Latitude = 3,
                Longitude = 4
            };

            //Change position of rover
            try
            {
                //response.Move = new Move[move.Length];
                //foreach (var m in move)
                //{
                //    response.Move.Append(m);
                //    response.StopPosition = new Position()
                //    {
                //        FacingDirection = "N",
                //        Latitude = 3,
                //        Longitude = 4
                //    };
                //}

                Emit(new MovedEvent(
                    position.FacingDirection, position.Longitude, position.Latitude
                ));
            }
            catch (Exception ex)
            {
                Emit(new MovedEvent(
                    position.FacingDirection, position.Longitude, position.Latitude
                ));
            }

            return ExecutionResult.Success();
        }

        public void Apply(MovedEvent aggregateEvent)
        {
            position = new Position()
            {
                Latitude = aggregateEvent.Latitude,
                Longitude = aggregateEvent.Longitude,
                FacingDirection = aggregateEvent.FacingDirection
            };
        }
    }
}
