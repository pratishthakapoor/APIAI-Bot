using Autofac;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Scorables;
using ServiceChatApp_APIAI_.Dialogs.ScorableDialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ServiceChatApp_APIAI_.Model
{
    public class GlobalMessageHandler : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder
                .Register(c => new ChatResetScorable(c.Resolve<IDialogTask>()))
                .As<IScorable<IMessageActivityMapper, double>>()
                .InstancePerLifetimeScope();
        }
    }
}