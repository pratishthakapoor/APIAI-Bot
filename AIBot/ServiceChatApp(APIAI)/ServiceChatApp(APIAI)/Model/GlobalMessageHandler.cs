using Autofac;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Scorables;
using Microsoft.Bot.Connector;
using ServiceChatApp_APIAI_.Dialogs.ScorableDialog;
using System.Reflection;

namespace ServiceChatApp_APIAI_
{
    internal class GlobalMessageHandler : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder
                .Register(c => new ChatResetScorable(c.Resolve<IDialogTask>()))
                .As<IScorable<IActivity, double>>()
                .InstancePerLifetimeScope();

            /**
             * Builder to register the MoreScorable.cs file
             **/

            builder
               .Register(c => new MoreRescorable(c.Resolve<IDialogTask>()))
               .As<IScorable<IActivity, double>>()
               .InstancePerLifetimeScope();

            /**
             * 
             **/

            builder
                .Register(c => new RaiseTicketScorable(c.Resolve<IDialogTask>()))
                .As<IScorable<IActivity, double>>()
                .InstancePerLifetimeScope();

        }
    }
}