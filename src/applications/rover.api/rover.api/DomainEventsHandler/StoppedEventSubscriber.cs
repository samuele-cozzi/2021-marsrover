using System;
using System.Threading;
using System.Threading.Tasks;
using EventFlow;
using EventFlow.Aggregates;
using EventFlow.Subscribers;
using Microsoft.Extensions.Hosting;
using rover.domain.Aggregates;
using rover.domain.Commands;
using rover.domain.DomainEvents;
using rover.domain.Models;
using rover.infrastructure.rabbitmq;

namespace rover.api.DomainEventsHandler
{
    public class StoppedEventSubscriber : IHostedService, IRabbitMqConsumerPersistanceService, 
        ISubscribeAsynchronousTo<StopAggregate, StopId, StoppedEvent>
    {
        private readonly ICommandBus _commandBus;
        
        public StoppedEventSubscriber(
            ICommandBus commandBus)
        {
            _commandBus = commandBus;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task HandleAsync(IDomainEvent<StopAggregate, StopId, StoppedEvent> domainEvent, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Location Updated for ");
            _commandBus.PublishAsync(
                new PositionCommand(
                    PositionId.New, 
                    new Position() { 
                        FacingDirection = domainEvent.AggregateEvent.FacingDirection, 
                        Latitude = domainEvent.AggregateEvent.Latitude, 
                        Longitude = domainEvent.AggregateEvent.Longitude }, 
                    domainEvent.AggregateEvent.IsBlocked, 
                    domainEvent.AggregateEvent.StartId,
                    domainEvent.AggregateEvent.Stop)
                , CancellationToken.None);

            if (!domainEvent.AggregateEvent.Stop && domainEvent.AggregateEvent.Longitude < 360)
            {
                if (domainEvent.AggregateEvent.IsBlocked)
                {
                    _commandBus.PublishAsync(
                    new StartCommand(StartId.New, new Moves[3] { Moves.r, Moves.f, Moves.l }, domainEvent.AggregateEvent.Stop), CancellationToken.None);
                }
                else
                {
                    _commandBus.PublishAsync(
                    new StartCommand(StartId.New, new Moves[4] { Moves.f, Moves.f, Moves.f, Moves.f }, domainEvent.AggregateEvent.Stop), CancellationToken.None);
                }
            }

            return Task.CompletedTask;
        }

    }
}
