using EventFlow;
using EventFlow.Aggregates;
using EventFlow.Subscribers;
using Microsoft.Extensions.Hosting;
using rover.application.Aggregates;
using rover.application.Commands;
using rover.application.Models;
using rover.infrastructure.rabbitmq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace rover.application.DomainEvents
{
    public class PositionChangedEventSubscriber : IHostedService, 
        IRabbitMqConsumerPersistanceService, ISubscribeAsynchronousTo<PositionAggregate, PositionId, PositionChangedEvent>
    {
        private readonly ICommandBus _commandBus;

        public PositionChangedEventSubscriber(
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

        public Task HandleAsync(IDomainEvent<PositionAggregate, PositionId, PositionChangedEvent> domainEvent, CancellationToken cancellationToken)
        {
            if (!domainEvent.AggregateEvent.Stop)
            {
                if (domainEvent.AggregateEvent.IsBlocked)
                {
                    _commandBus.PublishAsync(
                    new StartCommand(StartId.New, new string[2] { "r", "f" }, domainEvent.AggregateEvent.Stop), CancellationToken.None);
                }
                else
                {
                    _commandBus.PublishAsync(
                    new StartCommand(StartId.New, new string[4] { "f", "f", "f", "f" }, domainEvent.AggregateEvent.Stop), CancellationToken.None);
                }
            }

            return Task.CompletedTask;
        }
    }
}
