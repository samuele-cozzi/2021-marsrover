using System;
using System.Threading;
using System.Threading.Tasks;
using EventFlow;
using EventFlow.Configuration;
using EventFlow.Jobs;
using rover.domain.Aggregates;
using rover.domain.Commands;
using rover.domain.Models;

namespace rover.domain.Jobs
{
    public class HandlingRoverMessagesJob : IJob
    {
        public RoverAggregateId _id;
        public FacingDirections _facingDirection;
        public double _latitude;
        public double _longitude;
        public bool _isBlocked;
        public bool _stop;
        public double _coordinatePrecision;

        public HandlingRoverMessagesJob(
            RoverAggregateId id,
            FacingDirections facingDirection,
            double latitude,
            double longitude,
            bool isBlocked,
            bool stop,
            double coordinatePrecision
        )
        {
            _id = id;
            _facingDirection = facingDirection;
            _latitude = latitude;
            _longitude = longitude;
            _isBlocked = isBlocked;
            _stop = stop;
            _coordinatePrecision = coordinatePrecision;
        }

        public async Task ExecuteAsync(
            IResolver resolver,
            CancellationToken cancellationToken)
        {
            ICommandBus _commandBus = resolver.Resolve<ICommandBus>();

            await _commandBus.PublishAsync(
                new ChangePositionCommand(
                    _id,
                    new Position()
                    {
                        FacingDirection = _facingDirection,
                        Coordinate = new Coordinate()
                        {
                            Latitude = _latitude,
                            Longitude = _longitude
                        }
                    },
                    _isBlocked,
                    _stop)
                , CancellationToken.None);
        }
    }
}