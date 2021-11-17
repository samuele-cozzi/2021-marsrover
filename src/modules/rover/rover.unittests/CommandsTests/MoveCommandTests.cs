﻿using EventFlow;
using EventFlow.Aggregates;
using EventFlow.Queries;
using rover.domain.Aggregates;
using rover.domain.Commands;
using rover.domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace rover.unittests.CommandsTests
{
    public class MoveCommandTests
    {
        
        //[Theory]
        //[InlineData(new Moves[2] { Moves.f, Moves.f }, FacingDirections.E, 0, 2)]
        //public async void MoveCommand_Move_PositiveReadModel(Moves[] moves, FacingDirections direction, double Latitude, double Longitude)
        //{
        //    // Arrange
        //    var helper = new TestHelpers();
        //    var resolver = helper.Resolver_LandingLat0Long0FacE_Step1_ObstacleLat0Long1();
        //    ICommandBus _commandBus = resolver.Resolve<ICommandBus>();
        //    IQueryProcessor _queryProcessor = resolver.Resolve<IQueryProcessor>();
        //    var id = RoverPositionAggregateId.New;

        //    // Act
        //    await _commandBus.PublishAsync(new MoveCommand(id, moves, true), CancellationToken.None);


        //    var @aggregate = await aggregateStore.LoadAsync<ExampleAggregate, ExampleId>(exampleId, CancellationToken.None);
        //    PositionReadModel position = await _queryProcessor.ProcessAsync(new ReadModelByIdQuery<PositionReadModel>(id), CancellationToken.None).ConfigureAwait(false);

        //    // Assert
        //    Assert.Equal(position?.FacingDirection, direction);
        //    Assert.Equal(position?.Latitude, Latitude);
        //    Assert.Equal(position?.Longitude, Longitude);
        //}

    }
}