using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Builder.Scorables.Internals;
using Microsoft.Bot.Connector;
using ServiceChatApp_APIAI_.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace ServiceChatApp_APIAI_.Dialogs.ScorableDialog
{
    public class MoreRescorable : ScorableBase<IActivity, string, double>
    {
        IDialogTask dialogTask;

        public MoreRescorable(IDialogTask dialogTask)
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

            //this.dialogTask.Reset();

            IDialog<IMessageActivity> interruption = null;

            if (message != null)
            {
                var incomingMessage = message.Text.ToLowerInvariant();

                //var messageToSend = ResponseMessage.HelpMessage;
                var messageToSend = API_AI_Logger.API_Response(incomingMessage);
                var commonResponseDialog = new CommonnResponseDialog(messageToSend + "" + ResponseMessage.HelpMessage);
                interruption = commonResponseDialog.Void<object, IMessageActivity>();
                this.dialogTask.Call(interruption, null);
                await dialogTask.PollAsync(token);
            }
        }

        protected override async Task<string> PrepareAsync(IActivity item, CancellationToken token)
        {
            var message = item as IMessageActivity;

            if (message != null && !string.IsNullOrWhiteSpace(message.Text))
            {
                /*if (message.Text.Equals("what more", StringComparison.InvariantCultureIgnoreCase) || message.Text.Equals("else", StringComparison.InvariantCultureIgnoreCase) ||
                    message.Text.Equals("i need something more", StringComparison.InvariantCultureIgnoreCase) || message.Text.Equals("i need something else", StringComparison.InvariantCultureIgnoreCase)
                    || message.Text.Equals("what can you give me more", StringComparison.InvariantCultureIgnoreCase) || message.Text.Equals("i want something more",StringComparison.InvariantCultureIgnoreCase))*/
                if(Regex.IsMatch(message.Text, @"\bmore\b") || Regex.IsMatch(message.Text, @"\belse\b") || Regex.IsMatch(message.Text, @"\bneed\b"))
                {
                    return message.Text;
                }
            }
            return null;
        }
    }
}