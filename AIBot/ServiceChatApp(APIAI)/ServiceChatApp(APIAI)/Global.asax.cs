﻿using System.Web.Http;
using Microsoft.Bot.Builder.Azure;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Autofac;
using Microsoft.Bot.Connector;
using System.Reflection;
using System;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Builder.Autofac.Base;
using ServiceChatApp_APIAI_.Dialogs.ScorableDialog;

namespace ServiceChatApp_APIAI_
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            Conversation.UpdateContainer(
            builder =>
            {
                builder.RegisterModule(new AzureModule(Assembly.GetExecutingAssembly()));

                builder.RegisterModule(new ReflectionSurrogateModule());
                builder.RegisterModule<GlobalMessageHandler>();

                builder
                .RegisterKeyedType<StoreLastActivity, IBotToUser>()
                .InstancePerLifetimeScope();

                builder
                   .RegisterAdapterChain<IBotToUser>
                   (
                      typeof(AlwaysSendDirect_BotToUser),
                      typeof(AutoInputHint_BotToUser),
                      typeof(MapToChannelData_BotToUser),
                      typeof(StoreLastActivity),
                      typeof(LogBotToUser)
                   )
                   .InstancePerLifetimeScope();

                // Bot Storage: Here we register the state storage for your bot. 
                // Default store: volatile in-memory store - Only for prototyping!
                // We provide adapters for Azure Table, CosmosDb, SQL Azure, or you can implement your own!
                // For samples and documentation, see: [https://github.com/Microsoft/BotBuilder-Azure](https://github.com/Microsoft/BotBuilder-Azure)
                var store = new InMemoryDataStore();

                // Other storage options
                // var store = new TableBotDataStore("...DataStorageConnectionString..."); // requires Microsoft.BotBuilder.Azure Nuget package 
                // var store = new DocumentDbBotDataStore("cosmos db uri", "cosmos db key"); // requires Microsoft.BotBuilder.Azure Nuget package 

                builder.Register(c => store)
                    .Keyed<IBotDataStore<BotData>>(AzureModule.Key_DataStore)
                    .AsSelf()
                    .SingleInstance();

               // RegisterBotModules();

            });
        }

        /*private void RegisterBotModules()
        {
            Conversation.UpdateContainer(
                builder =>
                {
                    builder.RegisterModule(new ReflectionSurrogateModule());
                    builder.RegisterModule<GlobalMessageHandler>();

                    var store = new TableBotDataStore(ConfigurationManager.AppSettings["AzureWebJobsStorage"]);

                    builder.Register(c => store)
                            .Keyed<IBotDataStore<BotData>>(AzureModule.Key_DataStore)
                            .AsSelf()
                            .SingleInstance();

                    builder.Register(c => new CachingBotDataStore(store, CachingBotDataStoreConsistencyPolicy.ETagBasedConsistency))
                            .As<IBotDataStore<BotData>>()
                            .AsSelf()
                            .InstancePerLifetimeScope();
                });
        }*/
    }
}
