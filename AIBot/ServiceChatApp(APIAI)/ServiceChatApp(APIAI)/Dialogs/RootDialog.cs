using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiAiSDK;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Connector;

namespace ServiceChatApp_APIAI_.Dialogs
{

    [Serializable]
    public class RootDialog : IDialog<object>
    {
        string retry_response = API_AI_Logger.API_Response("retry");

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

            else if(action_response.Contains("RaiseTicket-repeat"))
            {

            }

            else
            {
                PromptDialog.Text(
                context,
                resume: MenuOption,
                prompt: "I can \n \n 1. Raise an incident ticket. \n \n 2. Check the status of previous raise ticket.",
                retry: "Please try agin, as some problem occured");
            }
            

            //context.Wait(MessageReceivedAsync);
        }

        private void NextCall(IDialogContext context)
        {
            PromptDialog.Confirm(
                    context,
                    resume: ResponseOption,
                    prompt: "Do you wish to check that out",
                    retry: retry_response
                    );
        }

        private async Task ResponseOption(IDialogContext context, IAwaitable<bool> result)
        {
            var confirmation = await result;
            PromptDialog.Text(
                context,
                resume: DisplayTicketStatus,
                prompt:"Please provide the ticket number for which you want to check the status",
                retry: retry_response);
        }

        private async Task DisplayTicketStatus(IDialogContext context, IAwaitable<string> result)
        {
            var incidentTokenNumber = await result;

            string statusDetail = Logger.RetrieveIncidentServiceNow(incidentTokenNumber);

            string statusDetails = Logger.RetrieveIncidentServiceNow(incidentTokenNumber);

            /**
             * The if- else- if condition to match the state of the incident token returned by the RetrieveIncidentSerivceNow method
             */

            if (statusDetails == "1")
            {
                var status = "Your token is created and is under review by our team.";
                string Notesresult = Logger.RetrieveIncidentWorkNotes(incidentTokenNumber);

                var replyMessage = context.MakeMessage();
                Attachment attachment = HeroCardDetails.GetReplyMessage(Notesresult, incidentTokenNumber, status);
                replyMessage.Attachments = new List<Attachment> { attachment };
                await context.PostAsync(replyMessage);

            }

            else if (statusDetails == "2")
            {
                var status = "Your ticket is in progress.";
                string Notesresult = Logger.RetrieveIncidentWorkNotes(incidentTokenNumber);

                var replyMessage = context.MakeMessage();
                Attachment attachment = HeroCardDetails.GetReplyMessage(Notesresult, incidentTokenNumber, status);
                replyMessage.Attachments = new List<Attachment> { attachment };
                await context.PostAsync(replyMessage);

            }

            else if (statusDetails == "3")
            {
                await context.PostAsync("Your ticket is been kept on hold.");


            }

            else if (statusDetails == "6")
            {
                var status = "Your ticket is resolved.";

                /**
                 * Retrieves the details from the resolve columns of SnowLogger class if the incident token is being resolved
                 **/

                string resolveDetails = Logger.RetrieveIncidentResolveDetails(incidentTokenNumber);
                var replyMessage = context.MakeMessage();
                Attachment attachment = HeroCardDetails.GetReplyMessage(resolveDetails, incidentTokenNumber, status);
                replyMessage.Attachments = new List<Attachment> { attachment };
                await context.PostAsync(replyMessage);
            }


            else if (statusDetails == "7")
            {
                var status = "Your ticket has been closed by our team";

                /**
                 * Retrieves the close_code from the SnowLogger class if the incident token is being closed
                 **/

                string resolveDetails = Logger.RetrieveIncidentCloseDetails(incidentTokenNumber);
                var replyMessage = context.MakeMessage();
                Attachment attachment = HeroCardDetails.GetReplyMessage(resolveDetails, incidentTokenNumber, status);
                replyMessage.Attachments = new List<Attachment> { attachment };
                //await context.PostAsync("Reasons for closing the ticket: " + resolveDetails);
                await context.PostAsync(replyMessage);
            }

            else
                await context.PostAsync("Our team cancelled your ticket");
        }

        private async Task MenuOption(IDialogContext context, IAwaitable<string> result)
        {
            var res = await result;

            string menu_response = API_AI_Logger.API_Response(res);

            await context.PostAsync(menu_response);
            context.Call(child: new TicketModel(), resume: childDialogcomplete);
        }

        private async Task childDialogcomplete(IDialogContext context, IAwaitable<object> result)
        {
            context.Done(this);
        }
    }
}