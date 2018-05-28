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
                Attachment attachment = HeroCardDetails.GetReplyMessage(resolveDetails + "\n" + Logger.RetrieveIncidentResolveDetails(incidentTokenNumber), incidentTokenNumber, status);
                replyMessage.Attachments = new List<Attachment> { attachment };
                //await context.PostAsync("Reasons for closing the ticket: " + resolveDetails);
                await context.PostAsync(replyMessage);
            }

            else if (statusDetails == "8")
            {
                await context.PostAsync("Our team cancelled your ticket");
            }

            else
            {
                await context.PostAsync("Please check the ticket details. There is some mistake");
            }

            context.Done(this);
        }
    }
}