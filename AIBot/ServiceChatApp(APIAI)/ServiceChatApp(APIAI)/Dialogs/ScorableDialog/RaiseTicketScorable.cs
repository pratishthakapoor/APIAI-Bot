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
    public class RaiseTicketScorable : ScorableBase<IActivity, string, double>
    {
        IDialogTask dialogTask;

        public RaiseTicketScorable(IDialogTask dialogTask)
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
            var ticketForm = new TicketModel();

            //var ticketForm = new RaiseDialog();

            var interruption = ticketForm.Void<object, IMessageActivity>();

            dialogTask.Call(interruption, null);

            await dialogTask.PollAsync(token);
        }

        protected override async Task<string> PrepareAsync(IActivity item, CancellationToken token)
        {
            var message = item as IMessageActivity;

            if (message != null && !string.IsNullOrWhiteSpace(message.Text))
            {
                /*API_AI_Logger.API_Connection_Action(message.Text);*/
                /*if (message.Text.Equals("Raise Ticket", StringComparison.InvariantCultureIgnoreCase) ||
                    message.Text.Equals("I want to raise a ticket", StringComparison.InvariantCultureIgnoreCase) ||
                    message.Text.Equals("Raise ticket for me", StringComparison.InvariantCultureIgnoreCase) ||
                    message.Text.Equals("Raise an ticket for me", StringComparison.InvariantCultureIgnoreCase) ||
                    message.Text.Equals("Raise a ticket", StringComparison.InvariantCultureIgnoreCase))*/
                //if(message.Text.Contains(API_AI_Logger.API_Connection_Action(message.Text)))
                if(Regex.IsMatch(message.Text, @"\bticket\b") || Regex.IsMatch(message.Text, @"\braise\b"))
                {
                    return message.Text;
                }
            }
            return null;

        }
    }
}