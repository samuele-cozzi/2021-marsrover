using EventFlow;
using EventFlow.Aggregates;
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
    public class CommandsTests
    {

        [Theory]
        [InlineData(new Moves[1] { Moves.f }, FacingDirections.E, 0, 1)]
        public async void MoveCommand_Move_PositiveAggregate(Moves[] moves, FacingDirections direction, double Latitude, double Longitude)
        {
            // Arrange
            var helper = new TestHelpers();
            var resolver = helper.Resolver_LandingLat0Long0FacE_Step1_ObstacleLat0Long2(Guid.NewGuid().ToString());
            ICommandBus _commandBus = resolver.Resolve<ICommandBus>();
            IAggregateStore aggregateStore = resolver.Resolve<IAggregateStore>();
            var id = RoverAggregateId.New;

            // Act
            var commandResult = await _commandBus.PublishAsync(new MoveCommand(id, moves, true), CancellationToken.None);
            var _aggregate = await aggregateStore.LoadAsync<RoverAggregate, RoverAggregateId>(id, CancellationToken.None);

            // Assert
            Assert.True(commandResult.IsSuccess);
            Assert.Equal(direction, _aggregate._position?.FacingDirection);
            Assert.Equal(Latitude, _aggregate._position?.Coordinate?.Latitude);
            Assert.Equal(Longitude, _aggregate._position?.Coordinate?.Longitude);
        }

        [Theory]
        [InlineData(FacingDirections.E, 0, 1, true)]
        [InlineData(FacingDirections.E, 0, 1, false)]
        public async void ChangePositionCommand_ChangedPosition_PositiveReadModel(FacingDirections direction, double Latitude, double Longitude, bool isBlocked)
        {
            // Arrange
            var helper = new TestHelpers();
            var resolver = helper.Resolver_LandingLat0Long0FacE_Step1_ObstacleLat0Long2(Guid.NewGuid().ToString());
            ICommandBus _commandBus = resolver.Resolve<ICommandBus>();
            IQueryProcessor _queryProcessor = resolver.Resolve<IQueryProcessor>();
            var id = RoverAggregateId.New;
            var position = new Position() { Coordinate = new Coordinate() { AngularPrecision = 1, Latitude = Latitude, Longitude = Longitude}, FacingDirection = direction };

            // Act
            var commandResult = await _commandBus.PublishAsync(new ChangePositionCommand(id, position, isBlocked, true), CancellationToken.None);
            PositionReadModel readModel = await _queryProcessor.ProcessAsync(new ReadModelByIdQuery<PositionReadModel>(id), CancellationToken.None).ConfigureAwait(false);

            // Assert
            Assert.True(commandResult.IsSuccess);
            Assert.Equal(direction, readModel?.FacingDirection);
            Assert.Equal(Latitude, readModel?.Latitude);
            Assert.Equal(Longitude, readModel?.Longitude);
            Assert.Equal(isBlocked, readModel?.IsBlocked);
        }

        [Theory]
        [InlineData(new Moves[1] { Moves.f }, "f")]
        [InlineData(new Moves[2] { Moves.f, Moves.f  }, "f-f")]
        [InlineData(new Moves[4] { Moves.f, Moves.r, Moves.b, Moves.l }, "f-r-b-l")]
        public async void StartCommand_Start_PositiveReadModel(Moves[] moves, string movesResult)
        {
            // Arrange
            var helper = new TestHelpers();
            var resolver = helper.Resolver_LandingLat0Long0FacE_Step1_ObstacleLat0Long2(Guid.NewGuid().ToString());
            ICommandBus _commandBus = resolver.Resolve<ICommandBus>();
            IQueryProcessor _queryProcessor = resolver.Resolve<IQueryProcessor>();
            var id = RoverAggregateId.New;

            // Act
            var commandResult = await _commandBus.PublishAsync(new StartCommand(id, moves, true), CancellationToken.None);
            StartReadModel readModel = await _queryProcessor.ProcessAsync(new ReadModelByIdQuery<StartReadModel>(id), CancellationToken.None).ConfigureAwait(false);

            // Assert
            Assert.True(commandResult.IsSuccess);
            Assert.Equal(movesResult, readModel?.Move);
        }

    }
}
