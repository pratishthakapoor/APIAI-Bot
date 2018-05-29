using System;
using System.Collections.Generic;
using Microsoft.Bot.Connector;

namespace ServiceChatApp_APIAI_
{
    internal class HeroCardDetails
    {
        internal static Attachment GetReplyMessage(string notesresult, string incidentTokenNumber, string status)
        {
            /**
             * Hero card creation 
             **/

            var heroCard = new HeroCard
            {
                //title for the given
                Title = "Progress details for the ticket " + incidentTokenNumber,
                // subtitle for the card
                Subtitle = status,
                //Detail text
                Text = "Latest work carried out on your raised ticket includes: " + "\n" + notesresult,
                //in case for other channel use
                /**
                 * Text = "Latest work carried out on your raised ticket includes:\n\n" + notesresult, ex : Text = "More words <br> New line <br> New line <b><font color = \"#11b92f\>GREEN</font></b></br></br>
                 **/
                //list of buttons
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl, "Need further details? ", value: "https://www.t-systems.hu/about-t-systems/customer-contact/service-desk"),
                    new CardAction(ActionTypes.OpenUrl, "Contact us at", value: "https://www.t-systems.com/de/en/contacts")}
            };
            return heroCard.ToAttachment();
        }
    }
}