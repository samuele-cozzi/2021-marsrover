using System;
using System.Threading;
using System.Threading.Tasks;
using EventFlow;
using EventFlow.Aggregates;
using EventFlow.Configuration;
using EventFlow.Jobs;
using EventFlow.Queries;
using rover.domain.Aggregates;
using rover.domain.Commands;
using rover.domain.DomainEvents;
using rover.domain.Models;
using rover.domain.Queries;

public class HandlingRoverMessagesJob : IJob
{
    public FacingDirections FacingDirection;
    public double Latitude;
    public double Longitude;
    public bool IsBlocked;
    public string startId;
    public bool Stop;
    public double CoordinatePrecision;

    public HandlingRoverMessagesJob (
        FacingDirections _FacingDirection,
        double _Latitude,
        double _Longitude,
        bool _IsBlocked,
        string _StartId,
        bool _Stop,
        double _CoordinatePrecision
    )
    {
        FacingDirection = _FacingDirection;
        Latitude = _Latitude;
        Longitude = _Longitude;
        IsBlocked = _IsBlocked;
        startId = _StartId;
        Stop = _Stop;
        CoordinatePrecision = _CoordinatePrecision;
    }

    public async Task ExecuteAsync(
        IResolver resolver,
        CancellationToken cancellationToken)
    {
        ICommandBus _commandBus = resolver.Resolve<ICommandBus>();
        IQueryProcessor _queryProcessor = resolver.Resolve<IQueryProcessor>();

        await _commandBus.PublishAsync(
            new PositionCommand(
                PositionId.New,
                new Position()
                {
                    FacingDirection = FacingDirection,
                    Coordinate = new Coordinate(){
                        Latitude = Latitude,
                        Longitude = Longitude
                    }
                },
                IsBlocked,
                startId,
                Stop)
            , CancellationToken.None);

        var landing = await _queryProcessor.ProcessAsync(new GetLandingPositionQuery(), CancellationToken.None);

        bool continueMoving = !Stop;
        bool isOnLandingLongitude = Longitude >= (landing .Longitude - CoordinatePrecision) &&
            Longitude <= (landing.Longitude + CoordinatePrecision);

        if (continueMoving && !isOnLandingLongitude)
        {
            if (IsBlocked)
            {
                var rnd = new Random();
                if(rnd.NextDouble() > 0.5){
                    await _commandBus.PublishAsync(
                    new StartCommand(StartId.New, new Moves[4] { Moves.r, Moves.f, Moves.l, Moves.f }, Stop), CancellationToken.None);
                }
                else
                {
                    await _commandBus.PublishAsync(
                    new StartCommand(StartId.New, new Moves[4] { Moves.l, Moves.f, Moves.r, Moves.f }, Stop), CancellationToken.None);
                }
            }
            else
            {
                await _commandBus.PublishAsync(
                new StartCommand(StartId.New, new Moves[4] { Moves.f, Moves.f, Moves.f, Moves.f }, Stop), CancellationToken.None);
            }
        }
    }
}