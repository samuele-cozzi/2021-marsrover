namespace rover_iot.DomainEventsHandler
{
    internal class StartEventSubscriber : IHostedService, IRabbitMqConsumerPersistanceService,
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
