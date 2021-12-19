//namespace rover.integration.tests.queriestest;

//public class PositionQueryTestEF
//{
//    [Fact]
//    public async void LandingPositionQuery_Get_RetunrnLanding()
//    {
//        // Arrange
//        var resolver = (new TestHelpers()).Resolver_LandingLat0Long0FacE_QueryEF();
//        IQueryProcessor _queryProcessor = resolver.Resolve<IQueryProcessor>();
//        IDbContextProvider<DBContextControlRoom> dbContextProvider = resolver.Resolve<IDbContextProvider<DBContextControlRoom>>();

//        using (var context = dbContextProvider.CreateContext())
//        {
//            context.Landing.Add(new LandingReadModel()
//            {
//                Latitude = 0,
//                Longitude = 0,
//                FacingDirection = FacingDirections.E,
//                AggregateId = RoverAggregateId.New.Value,
//                Timestamp = DateTime.UtcNow,
//                SequenceNumber = 1,
//            });
//            context.SaveChanges();
//        }

//        // Act
//        var result = await _queryProcessor.ProcessAsync(new GetLandingPositionQuery(), CancellationToken.None);

//        // Assert
//        Assert.Equal(0, result.Latitude);
//        Assert.Equal(0, result.Longitude);
//        Assert.Equal(FacingDirections.E, result.FacingDirection);
//    }

//    [Theory]
//    [InlineData(1, 10, 15, FacingDirections.S)]
//    [InlineData(2, -10, 115, FacingDirections.W)]
//    public async void LastPositionQuery_Get_RetunrnPosition(int id, double latitude, double longitude, FacingDirections direction)
//    {
//        // Arrange
//        var resolver = (new TestHelpers()).Resolver_LandingLat0Long0FacE_QueryEF();
//        IQueryProcessor _queryProcessor = resolver.Resolve<IQueryProcessor>();
//        IDbContextProvider<DBContextControlRoom> dbContextProvider = resolver.Resolve<IDbContextProvider<DBContextControlRoom>>();

//        using (var context = dbContextProvider.CreateContext())
//        {
//            context.Positions.Add(new PositionReadModel()
//            {
//                Latitude = latitude,
//                Longitude = longitude,
//                FacingDirection = direction,
//                AggregateId = RoverAggregateId.New.Value,
//                Timestamp = DateTime.UtcNow,
//                IsBlocked = false,
//                SequenceNumber = id,
//            });
//            context.SaveChanges();
//        }

//        // Act
//        var result = await _queryProcessor.ProcessAsync(new GetLastPositionQuery(), CancellationToken.None);



//        // Assert
//        Assert.Equal(latitude, result.Latitude);
//        Assert.Equal(longitude, result.Longitude);
//        Assert.Equal(direction, result.FacingDirection);

//    }
//}