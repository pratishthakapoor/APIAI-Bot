using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AdaptiveCards;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace ServiceChatApp_APIAI_
{
    [BotAuthentication]
    public class MessagesController : ApiController
    { 
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.GetActivityType() == ActivityTypes.Message)
            {
                var connectorClient = new ConnectorClient(new System.Uri(activity.ServiceUrl));
                Activity isTyping = activity.CreateReply("Bot is typing...");
                //Thread.Sleep(5000);
                isTyping.Type = ActivityTypes.Typing;
                await connectorClient.Conversations.ReplyToActivityAsync(isTyping);
                await Conversation.SendAsync(activity, () => new Dialogs.RootDialog());
            }
            else
            {
                HandleSystemMessageAsync(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private async Task<Activity> HandleSystemMessageAsync(Activity message)
        {
            string messageType = message.GetActivityType();
            if (messageType == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (messageType == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
                IConversationUpdateActivity conversationUpdateActivity = message as IConversationUpdateActivity;
                //IMessageActivity messageActivity = message as IMessageActivity;
                if (conversationUpdateActivity != null)
                {
                    ConnectorClient connector = new ConnectorClient(new System.Uri(message.ServiceUrl));
                    foreach (var memeber in conversationUpdateActivity.MembersAdded ?? System.Array.Empty<ChannelAccount>())
                    {
                        if (memeber.Id == conversationUpdateActivity.Recipient.Id)
                        {
                            /* var reply = ((Activity)conversationUpdateActivity).CreateReply($"Welcome, to Service Chat App");
                             await connector.Conversations.ReplyToActivityAsync(reply);*/
                            Activity reply = ((Activity)conversationUpdateActivity).CreateReply($"Welcome, to Service ChatBot");

                            AdaptiveCard adaptiveCard = new AdaptiveCard();

                            //Add an image to the card
                            adaptiveCard.Body.Add(new AdaptiveCards.Image()
                            {
                                Url = "https://ansiblergdiag813.blob.core.windows.net/chat-bot-images/images.png",
                                Size = ImageSize.Large,
                                Style = ImageStyle.Person,
                                HorizontalAlignment = HorizontalAlignment.Center,
                                Separation = SeparationStyle.Strong,
                                Type = "Image"
                            });

                            // Add text body to the adative card
                            adaptiveCard.Body.Add(new TextBlock()
                            {
                                Text = "I am ServiceChat Bot, designed to resolve your problems. ",
                                Size = TextSize.Large,
                                Weight = TextWeight.Lighter,
                                MaxLines = 2,
                                Wrap = true,
                                Color = TextColor.Accent,
                                IsSubtle = true
                            });

                            // Add text to start the conversation
                           /* adaptiveCard.Body.Add(new TextBlock()
                            {
                                Text = "1. I can raise a incident for you \n \n. 2. I can show the status of the ticket raised by you ",
                                Size = TextSize.Medium,
                                Weight = TextWeight.Lighter,
                                MaxLines = 2,
                                Wrap = true,
                                Color = TextColor.Dark,
                                IsSubtle = true
                            });*/

                            //Create an attachment for the adaptive card
                            Attachment attachment = new Attachment()
                            {
                                ContentType = AdaptiveCard.ContentType,
                                Content = adaptiveCard
                            };

                            reply.Attachments.Add(attachment);

                            var replytoconversation = await connector.Conversations.SendToConversationAsync(reply);

                        }
                    }
                }
            }
            else if (messageType == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (messageType == ActivityTypes.Typing)
            {
                // Handle knowing that the user is typing
            }
            else if (messageType == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}