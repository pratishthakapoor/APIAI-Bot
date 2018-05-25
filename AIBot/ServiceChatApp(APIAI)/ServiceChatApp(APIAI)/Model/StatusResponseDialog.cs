using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;

namespace ServiceChatApp_APIAI_.Dialogs.ScorableDialog
{
    internal class StatusResponseDialog : IDialog<object>
    {
        private string incomingMessage;

        public StatusResponseDialog(string incomingMessage)
        {
            this.incomingMessage = incomingMessage;
        }

        public async Task StartAsync(IDialogContext context)
        {
            var response = API_AI_Logger.API_Response(incomingMessage);

            RootDialog root = new RootDialog();
            root.StatusResponse(context, response);
        }
    }
}