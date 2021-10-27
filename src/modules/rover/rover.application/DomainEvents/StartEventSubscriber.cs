using EventFlow;
using EventFlow.Aggregates;
using EventFlow.Queries;
using EventFlow.Subscribers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using rover.application.Aggregates;
using rover.application.Commands;
using rover.application.Models;
using rover.domain.AggregateModels.Rover;
using rover.domain.Settings;
using rover.infrastructure.rabbitmq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace rover.application.DomainEvents
{
    public class StartEventSubscriber : IHostedService, IRabbitMqConsumerPersistanceService, 
        ISubscribeAsynchronousTo<StartAggregate, StartId, StartEvent>
    {
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _queryProcessor;
        private readonly RoverSettings _options;

        private static Position position;

        public StartEventSubscriber(
            ICommandBus commandBus,
            IQueryProcessor queryProcessor,
            IOptions<RoverSettings> options)
        {
            _commandBus = commandBus;
            _queryProcessor = queryProcessor;
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            position = _options.Landing;
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task HandleAsync(IDomainEvent<StartAggregate, StartId, StartEvent> domainEvent, CancellationToken cancellationToken)
        {
            bool isBlocked = false;
            foreach (var move in domainEvent.AggregateEvent.Move)
            {
                if (move == Moves.f || move == Moves.b)
                {
                    var id = MoveId.New;
                    var result = _commandBus.PublishAsync(new MoveCommand(id, position, move), CancellationToken.None).Result;
                    if (result.IsSuccess)
                    {
                        var model = _queryProcessor.ProcessAsync(new ReadModelByIdQuery<MoveReadModel>(id), CancellationToken.None).Result;
                        position.Latitude = model.Latitude;
                        position.Longitude = model.Longitude;
                    }
                    else
                    {
                        isBlocked = true;
                        break;
                    }
                    
                }
                if (move == Moves.l || move == Moves.r)
                {
                    var id = TurnId.New;

                    var result = _commandBus.PublishAsync(new TurnCommand(id, position.FacingDirection, move), CancellationToken.None).Result;
                    var model = _queryProcessor.ProcessAsync(new ReadModelByIdQuery<TurnReadModel>(id), CancellationToken.None).Result;
                    position.FacingDirection = model.FacingDirection;
                }
            }

            _commandBus.PublishAsync(
                new StopCommand(
                    StopId.New, 
                    domainEvent.AggregateIdentity,
                    position,
                    isBlocked,
                    domainEvent.AggregateEvent.Stop
                ), CancellationToken.None);


            return Task.CompletedTask;
        }

    }
}
