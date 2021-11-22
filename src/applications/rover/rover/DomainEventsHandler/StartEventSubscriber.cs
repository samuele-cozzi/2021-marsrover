using System;
using System.Threading;
using System.Threading.Tasks;
using EventFlow;
using EventFlow.Aggregates;
using EventFlow.Queries;
using EventFlow.Subscribers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using rover.domain.Aggregates;
using rover.domain.Commands;
using rover.domain.DomainEvents;
using rover.domain.Models;
using rover.domain.Settings;
using rover.infrastructure.rabbitmq;

namespace rover.DomainEventsHandler
{
    public class StartEventSubscriber : IHostedService, IRabbitMqConsumerPersistanceService, 
        ISubscribeAsynchronousTo<RoverAggregate, RoverAggregateId, StartedEvent>
    {
        private readonly ICommandBus _commandBus;
        
        public StartEventSubscriber(
            ICommandBus commandBus,
            IQueryProcessor queryProcessor,
            IOptions<RoverSettings> options)
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

        public Task HandleAsync(IDomainEvent<RoverAggregate, RoverAggregateId, StartedEvent> domainEvent, CancellationToken cancellationToken)
        {
            _commandBus.PublishAsync(
                new MoveCommand(
                    domainEvent.AggregateIdentity,
                    domainEvent.AggregateEvent.Move,
                    domainEvent.AggregateEvent.Stop
                ), CancellationToken.None);

            return Task.CompletedTask;
        }

    }
}
