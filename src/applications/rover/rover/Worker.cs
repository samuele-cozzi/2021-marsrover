using EventFlow;
using EventFlow.Extensions;
using EventFlow.MsSql;
using EventFlow.MsSql.Extensions;
using EventFlow.Queries;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using rover.application.Aggregates;
using rover.application.Commands;
using rover.application.DomainEvents;
using rover.application.Entities;
using rover.domain.AggregateModels.Rover;
using rover.Services.Interfaces;
using rover.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace rover
{
    public class Worker : IHostedService
    {
        private readonly IHostApplicationLifetime _hostLifetime;

        private readonly IWaitService _waitService;
        private readonly ILogger<Worker> _logger;
        private readonly RoverSettings _options;

        private int? _exitCode;

        public Worker(IWaitService service, IHostApplicationLifetime hostLifetime, ILogger<Worker> logger, IOptions<RoverSettings> options)
        {
            _waitService = service ?? throw new ArgumentNullException(nameof(service));
            _hostLifetime = hostLifetime ?? throw new ArgumentNullException(nameof(hostLifetime));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Start Worker");
            _logger.LogInformation($"Rover Landing Latidude: {_options.Landing.Latitude}");
            _logger.LogInformation($"Rover Landing Longitude: {_options.Landing.Longitude}");
            _logger.LogInformation($"Rover Landing Orientation: {_options.Landing.FacingDirection}");



            using (var resolver = EventFlowOptions.New
                .AddEvents(typeof(LandedEvent))
                .AddEvents(typeof(MovedEvent))
                .AddCommands(typeof(LandingCommand))
                .AddCommands(typeof(MoveCommand))
                .AddCommandHandlers(typeof(LandingCommandHandler))
                .AddCommandHandlers(typeof(MoveCommandHandler))
                //.AddSnapshots(typeof(CompetitionSnapshot))
                //.RegisterServices(sr => sr.Register(i => SnapshotEveryFewVersionsStrategy.Default))
                //.RegisterServices(sr => sr.Register(c => ConfigurationManager.ConnectionStrings["EventStore"].ConnectionString))
                //.AddSynchronousSubscriber<LandingAggregate, RoverId, LandedEvent, LandedEventSubscriber>()
                .UseConsoleLog()
                .UseMssqlReadModel<LandingReadModel>()
                //.UseInMemoryReadStoreFor<LandingReadModel>()
                //.UseInMemoryReadStoreFor<MoveReadModel>()
                .ConfigureMsSql(MsSqlConfiguration.New.SetConnectionString(_options.ConnectionStringReadModel))
                .CreateResolver())
            {
                // Create a new identity for our aggregate root
                var roverId = RoverId.New;

                var commandBus = resolver.Resolve<ICommandBus>();
                var executionResult = await commandBus.PublishAsync(
                    new LandingCommand(roverId, _options.Landing), CancellationToken.None);

                var queryProcessor = resolver.Resolve<IQueryProcessor>();
                var exampleReadModel = await queryProcessor.ProcessAsync(
                  new ReadModelByIdQuery<LandingReadModel>(roverId), CancellationToken.None);

                var executionResult2 = await commandBus.PublishAsync(
                    new LandingCommand(roverId, new Position() { FacingDirection="N",Latitude=20,Longitude=20}), CancellationToken.None);

                var executionResult1 = await commandBus.PublishAsync(
                    new MoveCommand(roverId, new string[2] { "f", "f" }), CancellationToken.None);

                //var executionResult2 = await commandBus.PublishAsync(
                //    new MoveCommand(roverId, new Move[2] { Move.f, Move.f }), CancellationToken.None);

                //var executionResult3 = await commandBus.PublishAsync(
                //    new MoveCommand(roverId, new Move[2] { Move.f, Move.f }), CancellationToken.None);

                var exampleReadModel1 = await queryProcessor.ProcessAsync(
                  new ReadModelByIdQuery<LandingReadModel>(roverId), CancellationToken.None);

                //// Define some important value
                //const string name = "test-competition";
                //const string name2 = "new-name";
                //const string user = "test-user";

                //// Resolve the command bus and use it to publish a command
                //var commandBus = resolver.Resolve<ICommandBus>();
                //var executionResult = commandBus.PublishAsync(new RegisterCompetitionCommand(exampleId, user, name), CancellationToken.None).Result;

                //executionResult = commandBus.PublishAsync(new CorrectCompetitionCommand(exampleId, name2), CancellationToken.None).Result;

                //ReadModel.MsSql.ReadModelConfiguration.Query(resolver, exampleId).Wait();
                //ReadModel.EntityFramework.ReadModelConfiguration.Query(resolver, exampleId).Wait();

                //var entry1Id = EntryId.New;
                //var entry2Id = EntryId.New;

                //commandBus.PublishAsync(new RecordEntryCommand(exampleId, entry1Id, "Discipline 1", "Name 1", 11111), CancellationToken.None).Wait();
                //commandBus.PublishAsync(new RecordEntryCommand(exampleId, entry2Id, "Discipline 2", "Name 2", 22222), CancellationToken.None).Wait();
                //commandBus.PublishAsync(new CorrectEntryTimeCommand(exampleId, entry1Id, 10000), CancellationToken.None).Wait();
                //commandBus.PublishAsync(new CorrectEntryTimeCommand(exampleId, entry2Id, 20000), CancellationToken.None).Wait();

                //for (int x = 1; x < 100; x++)
                //{
                //    commandBus.PublishAsync(new CorrectEntryTimeCommand(exampleId, entry2Id, 2000 + x), CancellationToken.None).Wait();
                //}

                //commandBus.PublishAsync(new DeleteCompetitionCommand(exampleId), CancellationToken.None).Wait();
            }



            //try
            //{
            //    while (true)
            //    {
            //        // 1. Wait command
            //        await _waitService.PerformWaitTask();

            //        // 2. read command
            //        Move[] command = new Move[3] { Move.f, Move.l, Move.f }; //TODO

            //        // 3. Foreach item array
            //        foreach(var move in command) {
            //            List<Move> moved = new List<Move>();
            //            try
            //            {
            //                // 3.a Obstacle detection
            //                // 3.b Move
            //                // 3.d.II send event KO
            //                moved.Add(move);
            //            }
            //            catch (Exception e) 
            //            {
            //                // 3.d.II send event KO
            //            }

            //            // 3.d.I send event OK
            //        }

            //        // 4. Wait command
            //        await _waitService.PerformWaitTask();
            //    }
            //    _exitCode = 0;
            //}
            //catch (OperationCanceledException)
            //{
            //    _logger?.LogInformation("The job has been killed with CTRL+C");
            //    _exitCode = -1;
            //}
            //catch (Exception ex)
            //{
            //    _logger?.LogError(ex, "An error occurred");
            //    _exitCode = 1;
            //}
            //finally
            //{
            //    _hostLifetime.StopApplication();
            //}
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Environment.ExitCode = _exitCode.GetValueOrDefault(-1);
            _logger?.LogInformation($"Shutting down the service with code {Environment.ExitCode}");
            return Task.CompletedTask;
        }
    }
}
