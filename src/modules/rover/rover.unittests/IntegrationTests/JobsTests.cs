using EventFlow;
using EventFlow.Aggregates;
using EventFlow.Jobs;
using EventFlow.Queries;
using rover.domain.Aggregates;
using rover.domain.Commands;
using rover.domain.Models;
using rover.domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace rover.unittests.IntegrationTests
{
    public class JobsTests
    {

        [Theory]
        [InlineData(new Moves[1] { Moves.f }, "f")]
        [InlineData(new Moves[2] { Moves.f, Moves.f  }, "f-f")]
        [InlineData(new Moves[4] { Moves.f, Moves.r, Moves.b, Moves.l }, "f-r-b-l")]
        public async void SendMessageToRoverJob_Start_PositiveReadModel(Moves[] moves, string movesResult)
        {
            // Arrange
            var helper = new TestHelpers();
            var resolver = helper.Resolver_Commands_LandingLat0Long0FacE_Step1_ObstacleLat0Long2();
            IJobScheduler _jobScheduler = resolver.Resolve<IJobScheduler>();
            IQueryProcessor _queryProcessor = resolver.Resolve<IQueryProcessor>();
            var id = RoverAggregateId.New;

            // Act
            var result = await _jobScheduler.ScheduleAsync(
                new SendMessageToRoverJob(id, moves, true),
                TimeSpan.FromSeconds(0),
                CancellationToken.None)
                .ConfigureAwait(false);

            StartReadModel readModel = await _queryProcessor.ProcessAsync(new ReadModelByIdQuery<StartReadModel>(id), CancellationToken.None).ConfigureAwait(false);

            // Assert
            Assert.Equal(movesResult, readModel?.Move);
        }

        [Theory]
        [InlineData(FacingDirections.E, 0, 1, true)]
        [InlineData(FacingDirections.E, 0, 1, false)]
        public async void HandlingRoverMessagesJob_ChangedPosition_PositiveReadModel(FacingDirections direction, double Latitude, double Longitude, bool isBlocked)
        {
            // Arrange
            var helper = new TestHelpers();
            var resolver = helper.Resolver_Commands_LandingLat0Long0FacE_Step1_ObstacleLat0Long2();
            IJobScheduler _jobScheduler = resolver.Resolve<IJobScheduler>();
            IQueryProcessor _queryProcessor = resolver.Resolve<IQueryProcessor>();
            var id = RoverAggregateId.New;
            var position = new Position() { Coordinate = new Coordinate() { AngularPrecision = 1, Latitude = Latitude, Longitude = Longitude}, FacingDirection = direction };

            // Act
            var result = _jobScheduler.ScheduleAsync(
                new HandlingRoverMessagesJob(
                    id,
                    direction, Latitude, Longitude, isBlocked, true, 1
                ),
                TimeSpan.FromSeconds(0),
                CancellationToken.None)
                .ConfigureAwait(false);
            PositionReadModel readModel = await _queryProcessor.ProcessAsync(new ReadModelByIdQuery<PositionReadModel>(id), CancellationToken.None).ConfigureAwait(false);

            // Assert
            Assert.Equal(direction, readModel?.FacingDirection);
            Assert.Equal(Latitude, readModel?.Latitude);
            Assert.Equal(Longitude, readModel?.Longitude);
            Assert.Equal(isBlocked, readModel?.IsBlocked);
        }

    }
}
