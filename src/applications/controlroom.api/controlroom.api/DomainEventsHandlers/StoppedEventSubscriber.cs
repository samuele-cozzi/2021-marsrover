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

namespace controlroom.api.DomainEventsHandlers
{
    public class StoppedEventSubscriber : IHostedService, IRabbitMqConsumerPersistanceService,
        ISubscribeAsynchronousTo<StopAggregate, StopId, StoppedEvent>
    {
        private readonly ICommandBus _commandBus;
        private static double landingLongitude;

        public StoppedEventSubscriber(
            ICommandBus commandBus)
        {
            _commandBus = commandBus;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            landingLongitude = 0;
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
                    new Position()
                    {
                        FacingDirection = domainEvent.AggregateEvent.FacingDirection,
                        Coordinate = new Coordinate(){
                            Latitude = domainEvent.AggregateEvent.Latitude,
                            Longitude = domainEvent.AggregateEvent.Longitude
                        }
                    },
                    domainEvent.AggregateEvent.IsBlocked,
                    domainEvent.AggregateEvent.StartId,
                    domainEvent.AggregateEvent.Stop)
                , CancellationToken.None);

            bool continueMoving = !domainEvent.AggregateEvent.Stop;
            bool isOnLandingLongitude = domainEvent.AggregateEvent.Longitude >= (landingLongitude - domainEvent.AggregateEvent.CoordinatePrecision) &&
                domainEvent.AggregateEvent.Longitude <= (landingLongitude + domainEvent.AggregateEvent.CoordinatePrecision);

            if (continueMoving && !isOnLandingLongitude)
            {
                if (domainEvent.AggregateEvent.IsBlocked)
                {
                    var rnd = new Random();
                    if(rnd.NextDouble() > 0.5){
                        _commandBus.PublishAsync(
                        new StartCommand(StartId.New, new Moves[4] { Moves.r, Moves.f, Moves.l, Moves.f }, domainEvent.AggregateEvent.Stop), CancellationToken.None);
                    }
                    else
                    {
                        _commandBus.PublishAsync(
                        new StartCommand(StartId.New, new Moves[4] { Moves.l, Moves.f, Moves.r, Moves.f }, domainEvent.AggregateEvent.Stop), CancellationToken.None);
                    }
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
