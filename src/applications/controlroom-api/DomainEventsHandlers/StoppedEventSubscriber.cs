namespace controlroom_api.DomainEventsHandlers
{
    public class StoppedEventSubscriber : IHostedService, IRabbitMqConsumerPersistanceService,
        ISubscribeAsynchronousTo<RoverAggregate, RoverAggregateId, MovedEvent>
    {
        private readonly IJobScheduler _jobScheduler;
        private readonly IntegrationSettings _options;

        public StoppedEventSubscriber(
            IJobScheduler jobScheduler,
            IOptions<IntegrationSettings> options)
        {
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

        public Task HandleAsync(IDomainEvent<RoverAggregate, RoverAggregateId, MovedEvent> domainEvent, CancellationToken cancellationToken)
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
