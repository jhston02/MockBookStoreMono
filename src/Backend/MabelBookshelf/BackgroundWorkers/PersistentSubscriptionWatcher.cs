﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Infrastructure.Infrastructure.EventStoreDb;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MabelBookshelf.BackgroundWorkers
{
    public class PersistentSubscriptionWatcher : BackgroundService
    {
        private readonly PersistantSubscriptionSettings configuration;
        private readonly PersistentSubscriptionEventStoreContext ctx;
        private readonly IServiceProvider services;

        public PersistentSubscriptionWatcher(IServiceProvider services,
            PersistantSubscriptionSettings configuration, PersistentSubscriptionEventStoreContext ctx)
        {
            this.services = services;
            this.configuration = configuration;
            this.ctx = ctx;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var sleepTimespan = new TimeSpan(0, 0, 10);

            if (configuration.Connections != null)
                foreach (var config in configuration.Connections)
                    for (var i = 0; i < config.Count; i++)
                        await Subscribe(config.GroupName, config.StreamName, config.GroupName + config.StreamName + i,
                            stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                var droppedStreams = ctx.GetDroppedStreams();
                foreach (var stream in droppedStreams)
                    await Subscribe(stream.Group, stream.StreamName, stream.Id, stoppingToken);
                await Task.Delay(sleepTimespan, stoppingToken);
            }
        }

        private async Task Subscribe(string groupName, string streamName, string subscriptionId,
            CancellationToken stoppingToken)
        {
            await ctx.Subscribe(groupName, streamName, subscriptionId,
                async x =>
                {
                    using (var scope = services.CreateScope())
                    {
                        var mediator =
                            scope.ServiceProvider
                                .GetRequiredService<IMediator>();
                        await mediator.Publish(x, stoppingToken);
                    }
                },
                stoppingToken);
        }
    }

    public class PersistantSubscriptionSettings
    {
        public List<Connection>? Connections { get; set; }
    }

    public class Connection
    {
        public string GroupName { get; set; } = "default";
        public string StreamName { get; set; } = "default";
        public int Count { get; set; }
    }
}