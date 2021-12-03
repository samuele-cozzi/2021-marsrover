﻿global using controlroom_api.Extensions;
global using controlroom_api.Services;
global using controlroom_api.DomainEventsHandlers;

global using Microsoft.Extensions.Options;
global using Serilog;
global using rover.domain.Settings;
global using rover.domain.Aggregates;
global using rover.domain.Commands;
global using rover.domain.DomainEvents;
global using rover.domain.Models;
global using rover.domain.Queries;
global using rover.domain.Services;
global using rover.domain.Jobs;
global using rover.infrastructure.ef;
global using rover.infrastructure.rabbitmq;
global using EventFlow.Configuration;
global using EventFlow.AspNetCore.Extensions;
global using EventFlow.Jobs;
global using EventFlow.Aggregates;
global using EventFlow.Subscribers;
global using EventFlow.DependencyInjection.Extensions;
global using EventFlow.Extensions;
global using EventFlow.RabbitMQ;
global using EventFlow.RabbitMQ.Extensions;
global using EventFlow.Hangfire.Extensions;
global using EventFlow.EntityFramework;
global using EventFlow.Queries;
global using Hangfire;
global using Hangfire.SqlServer;
global using EventFlow.MsSql;
global using EventFlow.MsSql.EventStores;
global using EventFlow.MsSql.Extensions;