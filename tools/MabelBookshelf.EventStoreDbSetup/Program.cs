﻿using System;
using CommandLine;
using EventStore.Client;

namespace MabelBookshelf.EventStoreDbSetup
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(o =>
                {
                    //TODO: Creds when we need real deployments
                    var settings = EventStoreClientSettings
                        .Create(o.ConnectionString);
                    var managementClient = new EventStoreProjectionManagementClient(settings);
                    managementClient.EnableAsync("$by_category").GetAwaiter().GetResult();

                    using var client = new EventStorePersistentSubscriptionsClient(
                        EventStoreClientSettings.Create(o.ConnectionString)
                    );
                    var pSettings =
                        new PersistentSubscriptionSettings(namedConsumerStrategy: "Pinned", resolveLinkTos: true);
                    client.CreateAsync(
                        "$ce-book",
                        "bookshelf-app",
                        pSettings).GetAwaiter().GetResult();

                    client.CreateAsync(
                        "$ce-bookshelf",
                        "bookshelf-app",
                        pSettings).GetAwaiter().GetResult();
                })
                .WithNotParsed(_ => Console.WriteLine("Not valid connections string"));
        }

        public class Options
        {
            [Option('c', "connection", Required = true, HelpText = "Connection string to evenstore db")]
            public string ConnectionString { get; set; }
        }
    }
}