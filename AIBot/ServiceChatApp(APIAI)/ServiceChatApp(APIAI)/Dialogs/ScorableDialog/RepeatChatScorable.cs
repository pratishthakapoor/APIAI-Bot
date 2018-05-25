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
    public class RepeatChatScorable : ScorableBase<IActivity, string, double>
    {
        IDialogTask dialogTask;

        public RepeatChatScorable(IDialogTask dialogTask)
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
            IMessageActivity activity = StoreLastActivity.RetrieveResponse();
            string messageToSend = activity.AsMessageActivity().Text.ToString();
            var lastMessage = new CommonnResponseDialog(messageToSend);

            //var ticketForm = new RaiseDialog();

            var interruption = lastMessage.Void<object, IMessageActivity>();

            dialogTask.Call(interruption, null);

            await dialogTask.PollAsync(token);
        }

        protected override async Task<string> PrepareAsync(IActivity item, CancellationToken token)
        {
            var message = item as IMessageActivity;

            if (message != null && !string.IsNullOrWhiteSpace(message.Text))
            {
                if(Regex.IsMatch(message.Text, @"\brepeat\b") || Regex.IsMatch(message.Text, @"\bagain\b") 
                    || message.Text.Equals("I didn't get that",StringComparison.InvariantCultureIgnoreCase))
                {
                    return message.Text;
                }
            }

            return null;
        }
    }
}