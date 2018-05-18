using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;

namespace ServiceChatApp_APIAI_.Dialogs
{
    [Serializable]

    public enum ContactOptions
    {
        Email,
        Phone
    }

    public enum Category
    {
        Inquiry,
        Software,
        Hadware,
        Newtwork,
        Database,
    }

    internal class TicketModel : IDialog<object>
    {
        private string description;
        private string short_description;
        private string detail_description;
        private string database_desc;
        private string server_desc;
        private string middleware_desc;
        private string contactOption;
        private string category_choice;
        private string incidentTokenNumber;

        string retry_response = API_AI_Logger.API_Response("retry");

        public async Task StartAsync(IDialogContext context)
        {
            PromptDialog.Text(
                context,
                resume: description_option,
                prompt: "Please provide a detailed description of the problem are you facing",
                retry: retry_response );
        }

        private async Task description_option(IDialogContext context, IAwaitable<string> Description)
        {
            var res = await Description;
            description = res;
            var key_phrases_extracted = await KeyPhraseAnalytics.ExtractPhraseAsync(description);
            if(key_phrases_extracted.Contains("server") || key_phrases_extracted.Contains("database"))
            {
                if(key_phrases_extracted.Contains("server"))
                {
                    //Console.WriteLine(key_phrases_extracted);
                    PromptDialog.Text(
                    context,
                    resume: server_description,
                    prompt: "Please provide server details",
                    retry: "retry_response}");
                }

                else if(key_phrases_extracted.Contains("database"))
                {
                    PromptDialog.Text(
                    context,
                    resume: database_details,
                    prompt: "Please provide database details",
                    retry: retry_response);
                }

                else
                {
                    PromptDialog.Text(
                    context,
                    resume: middleware_details,
                    prompt: "Please provide middleware details if any used by you",
                    retry: retry_response);
                }
               
            }

            PromptDialog.Choice(
                context,
                    options: (IEnumerable<ContactOptions>)Enum.GetValues(typeof(ContactOptions)),
                    resume: categoryChoice,
                    prompt: "Please chosse a category",
                    retry: retry_response
                );
            
        }

        private async Task categoryChoice(IDialogContext context, IAwaitable<ContactOptions> result)
        {
            var category = await result;
            category_choice = category.ToString();

            PromptDialog.Choice(
                    context,
                    options: (IEnumerable<ContactOptions>)Enum.GetValues(typeof(ContactOptions)),
                    resume: contact_choice,
                    prompt: "How do we contact you?",
                    retry: retry_response
                    );
        }

        private async Task middleware_details(IDialogContext context, IAwaitable<string> result)
        {
            middleware_desc = await result;
        }

        private async Task database_details(IDialogContext context, IAwaitable<string> result)
        {

            database_desc = await result;
        }

        private async Task server_description(IDialogContext context, IAwaitable<string> server_detail)
        {
            server_desc = await server_detail; 
        }

        private async Task contact_choice(IDialogContext context, IAwaitable<ContactOptions> contact_result)
        {
            var contactResponse = await contact_result;
            /**
             * Set the details in the method create incident calling method of SnowClass
             **/
            if (server_desc != null)
            {
                detail_description = description + server_desc;
                short_description = description;

                incidentTokenNumber = Logger.CreateServiceNow(detail_description, short_description, contactOption, category_choice);

                await context.PostAsync("An incident ticket is being raised for you\n\n following is the raised token number " + incidentTokenNumber);
                await context.PostAsync("Please take a note of this number as it is required for any future references");
            }

            else if(database_desc != null)
            {
                detail_description = description + database_desc;
                short_description = description;

                incidentTokenNumber = Logger.CreateServiceNow(detail_description, short_description, contactOption, category_choice);

                await context.PostAsync("An incident ticket is being raised for you\n\n following is the raised token number " + incidentTokenNumber);
                await context.PostAsync("Please take a note of this number as it is required for any future references");
            }

            else if(middleware_desc != null)
            {
                detail_description = description + middleware_desc;
                short_description = description;

                incidentTokenNumber = Logger.CreateServiceNow(detail_description, short_description, contactOption, category_choice);

                await context.PostAsync("An incident ticket is being raised for you\n\n following is the raised token number " + incidentTokenNumber);
                await context.PostAsync("Please take a note of this number as it is required for any future references");

            }

            else
            {
                detail_description = description;
                short_description = description;

                incidentTokenNumber = Logger.CreateServiceNow(detail_description, short_description, contactOption, category_choice);

                await context.PostAsync("An incident ticket is being raised for you\n\n following is the raised token number " + incidentTokenNumber);
                await context.PostAsync("Please take a note of this number as it is required for any future references");
            }
        }
    }
}