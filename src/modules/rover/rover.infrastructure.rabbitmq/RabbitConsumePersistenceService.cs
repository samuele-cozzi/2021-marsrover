using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using EventFlow.Aggregates;
using EventFlow.EventStores;
using EventFlow.RabbitMQ;
using EventFlow.RabbitMQ.Integrations;
using EventFlow.Subscribers;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using rover.domain.Settings;

namespace rover.infrastructure.rabbitmq
{
    public class RabbitConsumePersistenceService : IHostedService, IDisposable
    {
        private readonly IDispatchToEventSubscribers _dispatchToEventSubscribers;
        private readonly IEventJsonSerializer _eventJsonSerializer;
        private readonly IRabbitMqConnectionFactory _rabbitMqConnectionFactory;
        private readonly IntegrationSettings _options;


        public RabbitConsumePersistenceService(
            IRabbitMqConnectionFactory rabbitMqConnectionFactory,
            IEventJsonSerializer eventJsonSerializer,
            IDispatchToEventSubscribers dispatchToEventSubscribers,
            IOptions<IntegrationSettings> options)
        {
            _rabbitMqConnectionFactory = rabbitMqConnectionFactory;
            _eventJsonSerializer = eventJsonSerializer;
            _dispatchToEventSubscribers = dispatchToEventSubscribers;
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public void Dispose()
        {
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var connection =
                await _rabbitMqConnectionFactory.CreateConnectionAsync(new Uri(_options.RabbitMQConnectionString), cancellationToken);
            await connection.WithModelAsync(model => {
                model.ExchangeDeclare(_options.RabbitMQReadExchange, ExchangeType.Fanout);
                model.QueueDeclare(_options.RabbitMQQueue, false, false, true, null);
                model.QueueBind(_options.RabbitMQQueue, _options.RabbitMQReadExchange, "");

                var consume = new EventingBasicConsumer(model);
                consume.Received += (obj, @event) => {
                    var msg = CreateRabbitMqMessage(@event);
                    
                        var domainEvent = _eventJsonSerializer.Deserialize(msg.Message, new Metadata(msg.Headers));

                        _dispatchToEventSubscribers.DispatchToAsynchronousSubscribersAsync(domainEvent, cancellationToken);
                    
                };


                model.BasicConsume(_options.RabbitMQQueue, false, consume);
                return Task.CompletedTask;
            }, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private static RabbitMqMessage CreateRabbitMqMessage(BasicDeliverEventArgs basicDeliverEventArgs)
        {
            var headers = basicDeliverEventArgs.BasicProperties.Headers.ToDictionary(kv => kv.Key,
                kv => Encoding.UTF8.GetString((byte[])kv.Value));
            var message = Encoding.UTF8.GetString(basicDeliverEventArgs.Body);

            return new RabbitMqMessage(
                message,
                headers,
                new Exchange(basicDeliverEventArgs.Exchange),
                new RoutingKey(basicDeliverEventArgs.RoutingKey),
                new MessageId(basicDeliverEventArgs.BasicProperties.MessageId));
        }
    }
}
