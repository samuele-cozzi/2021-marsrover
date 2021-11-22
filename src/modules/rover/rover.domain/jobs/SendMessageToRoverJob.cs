using System;
using System.Threading;
using System.Threading.Tasks;
using EventFlow;
using EventFlow.Aggregates;
using EventFlow.Configuration;
using EventFlow.Jobs;
using EventFlow.Queries;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using rover.domain.Aggregates;
using rover.domain.Commands;
using rover.domain.DomainEvents;
using rover.domain.Models;
using rover.domain.Queries;
using rover.domain.Settings;

public class SendMessageToRoverJob : IJob
{
    public RoverAggregateId _id;
    public Moves[] _enumList;
    public bool _stop;
    
    public SendMessageToRoverJob (RoverAggregateId id, Moves[] enumList, bool stop)
    {
        _id = id;
        _enumList = enumList;
        _stop = stop;
    }

    public async Task ExecuteAsync(IResolver resolver, CancellationToken cancellationToken)
    {
        ICommandBus _commandBus = resolver.Resolve<ICommandBus>();
        var result = await _commandBus.PublishAsync(new StartCommand(_id, _enumList, _stop), cancellationToken);
        if(!result.IsSuccess){
            throw new Exception("Error while sending command to rover");
        }
    }
}