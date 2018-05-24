using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;
using ServiceChatApp_APIAI_.Dialogs;

namespace ServiceChatApp_APIAI_
{
    [Serializable]

    internal class ChatResponseDialog : IDialog<object>
    {
        private string messageToSend;
        private IMessageActivity activity;
        private IDialogTask dialogTask;

        public ChatResponseDialog(string messageToSend)
        {
            this.messageToSend = messageToSend;
        }

        public async Task StartAsync(IDialogContext context)
        {
            if (string.IsNullOrEmpty(messageToSend))
            {
               // await context.PostAsync(activity);
            }

            else
            {
                await context.PostAsync(messageToSend);
                context.Wait(MessageReceivedAsync);
                //context.Done(this);
            }
            //return Task.CompletedTask;
        } 

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var response = await result as Activity;

            if(response.Text.Contains("yes"))
            {
                //this.dialogTask.Reset();
                await context.PostAsync("The chat has been restarted....");
                RootDialog rootDialog = new RootDialog();
                rootDialog.MenuOption(context);
            }
        }
    }
}