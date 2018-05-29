using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ApiAiSDK;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Connector;
using ServiceChatApp_APIAI_.Internals;

namespace ServiceChatApp_APIAI_.Dialogs
{

    [Serializable]
    public class RootDialog : IDialog<object>
    {
        string retry_response = API_AI_Logger.API_Response("retry");
        private string messageToSend;
        IDialogContext context;

        public RootDialog()
        {

        }

        /*public RootDialog(string messageToSend)
        {
            this.messageToSend = messageToSend;
           //context.Wait(GlobalMessageHandlerDialog);
           //GlobalMessageHandlerDialog(messageToSend);
        }*/

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            // Calculate something for us to return
            //int length = (activity.Text ?? string.Empty).Length;

            // Return our reply to the user
            //await context.PostAsync($"You sent {activity.Text} which was {length} characters");

           string response = API_AI_Logger.API_Response(activity.Text);
           string action_response = API_AI_Logger.API_Connection_Action(activity.Text);

            await context.PostAsync(response);

            /*if(action_response.Contains("input.next"))
            {
                await context.PostAsync("\n" + ResponseMessage.HelpMessage);
            }*/
            if(action_response.Contains("RaiseTicket-next"))
            {
                //NextCall(context);
                PromptDialog.Confirm(
                   context,
                   resume: ResponseOption,
                   prompt: "Do you wish to check that out",
                   retry: retry_response
                   );
            }
            
            /*else if(action_response.Contains("CheckStatus-next"))
            {
                PromptDialog.Confirm(
                  context,
                  resume: NextCall,
                  prompt: "Do you wish to check that out",
                  retry: retry_response
                  );
            }*/

            /*else if(action_response.Contains("RaiseTicket-repeat"))
            {
                RepeatMessage(response);
            }*/

            else if(action_response.Contains("RaiseTicket-response"))
            {
                PromptDialog.Text(
                    context,
                    resume : MenuOptionDialog,
                    prompt:"",
                    retry: retry_response);
            }

            else if(action_response.Contains("input.checkstatus") | action_response.Contains("CheckStatus-custom"))
            {
                if (Regex.IsMatch(activity.Text, @"INC"))
                {
                    context.Call(new StatusDialog(activity.Text), ChildDialogcomplete);
                }
                else
                {
                    PromptDialog.Text(
                    context,
                    resume: DisplayTicketStatus,
                    prompt: "Provide a response",
                    retry: retry_response);
                }
            }

            else if(action_response.Contains("smalltalk.agent.can_you_help"))
            {
                await context.PostAsync(response + "\n" + ResponseMessage.HelpMessage);
            }

            else if(Regex.IsMatch(action_response, "smalltalk.agent\b"))
            {
                await context.PostAsync("");
            }

            else
            {
                MenuOption(context);  
            }
            //context.Wait(MessageReceivedAsync);
        }

        /*private async Task NextCall(IDialogContext context, IAwaitable<bool> result)
        {
            var confirmation = await result;
            if(confirmation == true)
            {
                context.Call(new TicketModel(), ChildDialogcomplete);
            }

            else
            {
                await context.PostAsync("I am this much to offer you today. See you later");
            }
        }*/

        /*private async void RepeatMessage(string response)
        {
            await context.PostAsync(response);
        }*/

        internal void MenuOption(IDialogContext context)
        {
            PromptDialog.Text(
                context,
                resume: MenuOptionDialog,
                prompt: "I can \n \n 1. Raise an incident ticket. \n \n 2. Check the status of previous raise ticket.",
                retry: "Please try agin, as some problem occured");
        }

      /* private void NextCall(IDialogContext context)
        {
            PromptDialog.Confirm(
                    context,
                    resume: ResponseOption,
                    prompt: "Do you wish to check that out",
                    retry: retry_response
                    );
        }*/

        private async Task ResponseOption(IDialogContext context, IAwaitable<bool> result)
        {
            var confirmation = await result;
            if (confirmation == true)
            {
                PromptDialog.Text(
                    context,
                    resume: DisplayTicketStatus,
                    prompt: "Please provide the ticket number for which you want to check the status",
                    retry: retry_response);
            }

            else
            {
                await context.PostAsync("I am this much to offer you today. See you later");
            }
        }

        private async Task DisplayTicketStatus(IDialogContext context, IAwaitable<string> result)
        {
            var incidentTokenNumber = await result;

            context.Call(new StatusDialog(incidentTokenNumber), ChildDialogcomplete);

        }

        private async Task MenuOptionDialog(IDialogContext context, IAwaitable<string> result)
        {
            var res = await result;

            string menu_response = API_AI_Logger.API_Response(res);
            string intent_response = API_AI_Logger.API_Connection_Action(res);
            if(intent_response.Contains("input.checkstatus"))
            {
                StatusResponse(context, menu_response);

            }
            else if(intent_response.Contains("input.raise_ticket_response"))
            {
                await context.PostAsync(menu_response);
                context.Call(child: new TicketModel(), resume: ChildDialogcomplete);

                if(intent_response.Contains("RaiseTicket-next"))
                {
                    PromptDialog.Confirm(
                    context,
                    resume: ResponseOption,
                     prompt: "Do you wish to check that out",
                     retry: retry_response
                    );
                }
            }
            else
            {
                await context.PostAsync(menu_response);
            }
        }

        public void StatusResponse(IDialogContext context, string menu_response)
        {
            PromptDialog.Text(
                     context,
                     resume: DisplayTicketStatus,
                     prompt: menu_response,
                     retry: retry_response
                    );
        }

        private async Task ChildDialogcomplete(IDialogContext context, IAwaitable<object> result)
        {
            context.Done(this);
        }
    }
}