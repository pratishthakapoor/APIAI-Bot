﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Builder.Scorables.Internals;
using Microsoft.Bot.Connector;

namespace ServiceChatApp_APIAI_
{
    internal class ChatResetScorable : ScorableBase<IActivity, string, double>
    {
        private IDialogTask dialogTask;

        public ChatResetScorable(IDialogTask dialogTask)
        {
            SetField.NotNull(out this.dialogTask, nameof(dialogTask), dialogTask);
        }

        protected override Task DoneAsync(IActivity item, string state, CancellationToken token)
        {
            return Task.CompletedTask;
        }

        protected override double GetScore(IActivity item, string state)
        {
            return 1.0;
        }

        protected override bool HasScore(IActivity item, string state)
        {
            return state != null;
        }

        protected override async Task PostAsync(IActivity item, string state, CancellationToken token)
        {
            this.dialogTask.Reset();
        }

        protected override async Task<string> PrepareAsync(IActivity item, CancellationToken token)
        {
            var message = item as IMessageActivity;

            if (message != null && !string.IsNullOrWhiteSpace(message.Text))
            {
                if (message.Text.Equals("cancel", StringComparison.InvariantCultureIgnoreCase) || message.Text.Equals("reset", StringComparison.InvariantCultureIgnoreCase) ||
                    message.Text.Equals("restart", StringComparison.InvariantCultureIgnoreCase) || message.Text.Equals("start again", StringComparison.InvariantCultureIgnoreCase))
                {
                    return message.Text;
                }
            }
            return null;
        }
    }
}