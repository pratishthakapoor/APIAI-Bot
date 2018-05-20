using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using ServiceChatApp_APIAI_.Dialogs;
using System.Threading.Tasks;

namespace ServiceChatApp_APIAI_
{
    internal class CommonnResponseDialog : IDialog<object>
    {
        private string messageToSend;
        private Activity activity;

        public CommonnResponseDialog(string messageToSend)
        {
            this.messageToSend = messageToSend;
        }

        public async Task StartAsync(IDialogContext context)
        {
            if (string.IsNullOrEmpty(messageToSend))
            {
                await context.PostAsync(activity);
            }
            else
            {
                await context.PostAsync(messageToSend);
                RootDialog rootDialog = new RootDialog();
                rootDialog.MenuOption(context);
            }
            context.Done<object>(null);
        }
    }
}