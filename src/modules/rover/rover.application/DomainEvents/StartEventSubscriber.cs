using EventFlow;
using EventFlow.Aggregates;
using EventFlow.Subscribers;
using Microsoft.Extensions.Hosting;
using rover.application.Aggregates;
using rover.application.Commands;
using rover.application.Models;
using rover.domain.AggregateModels.Rover;
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
        private static int i;
        private readonly ICommandBus _commandBus;
        
        public StartEventSubscriber(
            ICommandBus commandBus)
        {
            _commandBus = commandBus;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            i = 1;
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task HandleAsync(IDomainEvent<StartAggregate, StartId, StartEvent> domainEvent, CancellationToken cancellationToken)
        {
            i++;
            Console.WriteLine($"Location Updated for {i}");
            _commandBus.PublishAsync(
                new StopCommand(
                    StopId.New, 
                    domainEvent.AggregateIdentity,
                    new Position() { FacingDirection = "S", Latitude = 100, Longitude = 100 }, 
                    false,
                    domainEvent.AggregateEvent.Stop
                ), CancellationToken.None);


            return Task.CompletedTask;
        }

    }
}
