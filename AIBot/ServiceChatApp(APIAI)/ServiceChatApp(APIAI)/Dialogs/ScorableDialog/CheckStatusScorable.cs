using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Builder.Scorables.Internals;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace ServiceChatApp_APIAI_.Dialogs.ScorableDialog
{
    public class CheckStatusScorable : ScorableBase<IActivity, string, double>
    {

        IDialogTask dialogTask;

        public CheckStatusScorable(IDialogTask dialogTask)
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
            var message = item as IMessageActivity;

            this.dialogTask.Reset();

            IDialog<IMessageActivity> interruption = null;

            if (message != null)
            {
                var incomingMessage = message.Text.ToLowerInvariant();
                //var messageToSend = API_AI_Logger.API_Response(incomingMessage);
                var commonResponseDialog = new StatusResponseDialog(incomingMessage);
                interruption = commonResponseDialog.Void<object, IMessageActivity>();
                this.dialogTask.Call(interruption, null);
                await dialogTask.PollAsync(token);
            }
        }

        protected override async Task<string> PrepareAsync(IActivity item, CancellationToken token)
        {
            var message = item as IMessageActivity;

            if(message != null && !string.IsNullOrWhiteSpace(message.Text))
            {
                if(Regex.IsMatch(message.Text, @"\bstatus\b") || Regex.IsMatch(message.Text, @"\bcheck\b") || Regex.IsMatch(message.Text, @"\bprevious\b"))
                {
                    return message.Text;
                }
            }

            return null;

        }
    }
}