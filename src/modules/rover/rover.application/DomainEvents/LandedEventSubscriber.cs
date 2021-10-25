using EventFlow.Aggregates;
using EventFlow.Subscribers;
using rover.application.Aggregates;
using rover.application.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace rover.application.DomainEvents
{
    public class LandedEventSubscriber : ISubscribeSynchronousTo<LandingAggregate, RoverId, LandedEvent>
    {
        public Task HandleAsync(IDomainEvent<LandingAggregate, RoverId, LandedEvent> domainEvent, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
