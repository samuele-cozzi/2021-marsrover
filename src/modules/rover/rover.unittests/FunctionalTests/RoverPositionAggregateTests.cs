using EventFlow.Aggregates;
using EventFlow.Queries;
using rover.domain.Aggregates;
using rover.domain.Models;
using rover.domain.Queries;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace rover.unittests.FinctionalTests
{
    public class RoverPositionAggregateTests
    {
        [Theory]
        [InlineData(new Moves[1] { Moves.f }, FacingDirections.E, 0, 1)]
        [InlineData(new Moves[1] { Moves.b }, FacingDirections.E, 0, -1)]
        [InlineData(new Moves[1] { Moves.r }, FacingDirections.S, 0, 0)]
        [InlineData(new Moves[2] { Moves.r, Moves.r }, FacingDirections.W, 0, 0)]
        [InlineData(new Moves[3] { Moves.r, Moves.r, Moves.r }, FacingDirections.N, 0, 0)]
        [InlineData(new Moves[4] { Moves.r, Moves.r , Moves.r, Moves.r }, FacingDirections.E, 0, 0)]
        [InlineData(new Moves[1] { Moves.l }, FacingDirections.N, 0, 0)]
        [InlineData(new Moves[2] { Moves.l, Moves.l }, FacingDirections.W, 0, 0)]
        [InlineData(new Moves[3] { Moves.l, Moves.l, Moves.l }, FacingDirections.S, 0, 0)]
        [InlineData(new Moves[4] { Moves.l, Moves.l, Moves.l, Moves.l }, FacingDirections.E, 0, 0)]
        //[InlineData(new Moves[2] { Moves.f, Moves.f }, FacingDirections.E, 0, 2)]
        [InlineData(new Moves[2] { Moves.r, Moves.f }, FacingDirections.S, -1, 0)]
        [InlineData(new Moves[2] { Moves.l, Moves.f }, FacingDirections.N, 1, 0)]
        public async void RoverPositionAggregate_Move_CheckNewPosition(Moves[] moves, FacingDirections direction, double Latitude, double Longitude)
        {
            // Arrange
            var helper = new TestHelpers();
            var resolver = helper.Resolver_LandingLat0Long0FacE_Step1_ObstacleLat0Long2(Guid.NewGuid().ToString());
            IAggregateStore aggregateStore = resolver.Resolve<IAggregateStore>();
            var id = RoverAggregateId.New;

            // Act
            var _aggregate = await aggregateStore.LoadAsync<RoverAggregate, RoverAggregateId>(id, CancellationToken.None);
            var result = _aggregate.Move(moves, true);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(direction, _aggregate._position?.FacingDirection);
            Assert.Equal(Latitude, _aggregate._position?.Coordinate?.Latitude);
            Assert.Equal(Longitude, _aggregate._position?.Coordinate?.Longitude);
        }

        [Theory]
        [InlineData(new Moves[2] { Moves.f, Moves.f }, FacingDirections.E, 0, 1)]
        public async void RoverPositionAggregate_Move_CheckBlock(Moves[] moves, FacingDirections direction, double Latitude, double Longitude)
        {
            // Arrange
            var helper = new TestHelpers();
            var resolver = helper.Resolver_LandingLat0Long0FacE_Step1_ObstacleLat0Long2(Guid.NewGuid().ToString());
            IAggregateStore aggregateStore = resolver.Resolve<IAggregateStore>();
            var id = RoverAggregateId.New;

            // Act
            var _aggregate = await aggregateStore.LoadAsync<RoverAggregate, RoverAggregateId>(id, CancellationToken.None);
            var result = _aggregate.Move(moves, true);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(direction, _aggregate._position?.FacingDirection);
            Assert.Equal(Latitude, _aggregate._position?.Coordinate?.Latitude);
            Assert.Equal(Longitude, _aggregate._position?.Coordinate?.Longitude);
        }

        [Theory]
        [InlineData(FacingDirections.E, 0, 0, false)]
        [InlineData(FacingDirections.E, 0, 0, true)]
        [InlineData(FacingDirections.N, 1, 1, false)]
        [InlineData(FacingDirections.S, -1, -1, true)]
        public async void RoverPositionAggregate_ChangePosition_CheckNewPosition(FacingDirections direction, double Latitude, double Longitude, bool isBlocked)
        {
            // Arrange
            var helper = new TestHelpers();
            var resolver = helper.Resolver_LandingLat0Long0FacE_Step1_ObstacleLat0Long2(Guid.NewGuid().ToString());
            IAggregateStore aggregateStore = resolver.Resolve<IAggregateStore>();
            IQueryProcessor _queryProcessor = resolver.Resolve<IQueryProcessor>();
            var id = RoverAggregateId.New;
            Position position = new Position() { FacingDirection = direction, Coordinate = new Coordinate() { Latitude = Latitude, Longitude = Longitude } };

            // Act
            var _aggregate = await aggregateStore.LoadAsync<RoverAggregate, RoverAggregateId>(id, CancellationToken.None);
            var result = _aggregate.ChangePosition(position, isBlocked, true);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(direction, _aggregate._position?.FacingDirection);
            Assert.Equal(Latitude, _aggregate._position?.Coordinate?.Latitude);
            Assert.Equal(Longitude, _aggregate._position?.Coordinate?.Longitude);
        }
    }
}
