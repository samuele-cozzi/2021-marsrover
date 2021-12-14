// using EventFlow.EntityFramework;
// using EventFlow.Queries;
// using rover.domain.Aggregates;
// using rover.domain.Models;
// using rover.domain.Queries;
// using rover.infrastructure.ef;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Threading;
// using System.Threading.Tasks;
// using Xunit;

// namespace rover.unittests.QueriesTests
// {
//     public class PositionQueryTestsDapper
//     {   
//         [Fact]
//         public async void LandingPositionQuery_Get_RetunrnLanding()
//         {
//             // Arrange
//             var resolver = (new TestHelpers()).Resolver_LandingLat0Long0FacE_QueryDapper();
//             IQueryProcessor _queryProcessor = resolver.Resolve<IQueryProcessor>();

//             // Act
//             var result = await _queryProcessor.ProcessAsync(new GetLandingPositionQuery(), CancellationToken.None);

//             // Assert
//             Assert.Null(result);
//         }

//         [Theory]
//         [InlineData(10,15, FacingDirections.S)]
//         [InlineData(-10, 115, FacingDirections.W)]
//         public async void LastPositionQuery_Get_RetunrnPosition(double latitude, double longitude, FacingDirections direction)
//         {
//             // Arrange
//             var resolver = (new TestHelpers()).Resolver_LandingLat0Long0FacE_QueryDapper();
//             IQueryProcessor _queryProcessor = resolver.Resolve<IQueryProcessor>();
//             IDbContextProvider<DBContextControlRoom> dbContextProvider = resolver.Resolve<IDbContextProvider<DBContextControlRoom>>();

//             using (var context = dbContextProvider.CreateContext())
//             {
//                 context.Positions.Add(new PositionReadModel()
//                 {
//                     Latitude = latitude,
//                     Longitude = longitude,
//                     FacingDirection = direction,
//                     AggregateId = RoverAggregateId.New.Value,
//                     Timestamp = DateTime.UtcNow,
//                     IsBlocked = false,
//                     SequenceNumber = 1,
//                 });
//                 context.SaveChanges();
//             }

//             // Act
//             var result = await _queryProcessor.ProcessAsync(new GetLastPositionQuery(), CancellationToken.None);
//             using (var context = dbContextProvider.CreateContext())
//             {
//                 context.Database.EnsureDeleted();
//             }

//             // Assert
//             Assert.Equal(latitude, result.Latitude);
//             Assert.Equal(longitude, result.Longitude);
//             Assert.Equal(direction, result.FacingDirection);

//         }
//     }
// }
