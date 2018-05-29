using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace ServiceChatApp_APIAI_.Dialogs
{
    internal class StatusDialog : IDialog<object>
    {
        private string incidentTokenNumber;

        public StatusDialog(string incidentTokenNumber)
        {
            this.incidentTokenNumber = incidentTokenNumber;
        }

        public async Task StartAsync(IDialogContext context)
        {
            //string statusDetail = Logger.RetrieveIncidentServiceNow(incidentTokenNumber);

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
                //context.Done(this);
            }

            else if (statusDetails == "2")
            {
                var status = "Your ticket is in progress.";
                string Notesresult = Logger.RetrieveIncidentWorkNotes(incidentTokenNumber);

                var replyMessage = context.MakeMessage();
                Attachment attachment = HeroCardDetails.GetReplyMessage(Notesresult, incidentTokenNumber, status);
                replyMessage.Attachments = new List<Attachment> { attachment };
                await context.PostAsync(replyMessage);
               // context.Done(this);

            }

            else if (statusDetails == "3")
            {
                await context.PostAsync("Your ticket is been kept on hold.");
                //context.Done(this);
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
                //context.Done(this);
            }


            else if (statusDetails == "7")
            {
                var status = "Your ticket has been closed by our team";

                /**
                 * Retrieves the close_code from the SnowLogger class if the incident token is being closed
                 **/

                string resolveDetails = Logger.RetrieveIncidentCloseDetails(incidentTokenNumber);
                var replyMessage = context.MakeMessage();
                Attachment attachment = HeroCardDetails.GetReplyMessage(resolveDetails + "\n" + Logger.RetrieveIncidentResolveDetails(incidentTokenNumber), incidentTokenNumber, status);
                replyMessage.Attachments = new List<Attachment> { attachment };
                //await context.PostAsync("Reasons for closing the ticket: " + resolveDetails);
                await context.PostAsync(replyMessage);
                //context.Done(this);
            }

            else if (statusDetails == "8")
            {
                await context.PostAsync("Our team cancelled your ticket");
                //context.Done(this);
            }

            else
            {
                await context.PostAsync("Please check the ticket details. There is some mistake");

                await context.PostAsync("Please provide correct incident ticket detail");

                RootDialog dialog = new RootDialog();
                await dialog.StartAsync(context);
                //await StartAsync(context);
            }

            //context.Wait(MessageRecievedAsync);
            PromptDialog.Text(
                context,
                resume: MessageRecievedAsync,
                prompt: "The above shows a detail description of the requested ticket",
                retry: "Please try again later");
        }

        private async Task MessageRecievedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var response = await result;

            string status_action = API_AI_Logger.API_Connection_Action(response.ToString());

            string status_response = API_AI_Logger.API_Response(response.ToString());

            await context.PostAsync("If you have any issue then i can take you  to the raise ticket option");

            PromptDialog.Confirm(
                context,
                resume: NextCall,
                prompt: "Do you wish to check that out",
                retry: "Please try again later"
                );

        }

        private async Task NextCall(IDialogContext context, IAwaitable<bool> result)
        {
            var confirmation = await result;
            if (confirmation == true)
            {
                context.Call(new TicketModel(), ChildDialogcomplete);
            }

            else
            {
                await context.PostAsync("I am this much to offer you today. See you later");
            }
        }

        private async Task ChildDialogcomplete(IDialogContext context, IAwaitable<object> result)
        {
            context.Done(this);
        }
    }
}