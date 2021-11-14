using System;
using System.Threading;
using System.Threading.Tasks;
using EventFlow;
using EventFlow.Aggregates;
using EventFlow.Jobs;
using EventFlow.Subscribers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using rover.domain.Aggregates;
using rover.domain.Commands;
using rover.domain.DomainEvents;
using rover.domain.Models;
using rover.domain.Settings;
using rover.infrastructure.rabbitmq;

namespace controlroom.api.DomainEventsHandlers
{
    public class StoppedEventSubscriber : IHostedService, IRabbitMqConsumerPersistanceService,
        ISubscribeAsynchronousTo<RoverPositionAggregate, RoverPositionAggregateId, StoppedEvent>
    {
        private readonly ICommandBus _commandBus;
        private readonly IJobScheduler _jobScheduler;
        private readonly IntegrationSettings _options;

        public StoppedEventSubscriber(
            ICommandBus commandBus,
            IJobScheduler jobScheduler,
            IOptions<IntegrationSettings> options)
        {
            _commandBus = commandBus;
            _jobScheduler = jobScheduler;
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task HandleAsync(IDomainEvent<RoverPositionAggregate, RoverPositionAggregateId, StoppedEvent> domainEvent, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Location Updated for ");

            var result = _jobScheduler.ScheduleAsync(
                new HandlingRoverMessagesJob(
                    domainEvent.AggregateIdentity,
                    domainEvent.AggregateEvent.FacingDirection,
                    domainEvent.AggregateEvent.Latitude,
                    domainEvent.AggregateEvent.Longitude,
                    domainEvent.AggregateEvent.IsBlocked,
                    domainEvent.AggregateEvent.Stop,
                    domainEvent.AggregateEvent.CoordinatePrecision
                ),
                TimeSpan.FromSeconds(_options.TimeDistanceOfMessageInSeconds),
                CancellationToken.None)
                .ConfigureAwait(false);

            return Task.CompletedTask;
        }

    }
}
