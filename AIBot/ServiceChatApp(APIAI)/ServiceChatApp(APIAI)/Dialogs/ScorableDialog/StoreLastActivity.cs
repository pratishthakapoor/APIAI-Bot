using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Connector;

namespace ServiceChatApp_APIAI_.Dialogs.ScorableDialog
{
    internal class StoreLastActivity : IBotToUser
    {
        private readonly IBotToUser inner;
        public static IMessageActivity Message;

        public StoreLastActivity(IBotToUser inner)
        {
            SetField.NotNull(out this.inner, nameof(inner), inner);
        }

        public IMessageActivity MakeMessage()
        {
            return this.inner.MakeMessage();
        }

        public async Task PostAsync(IMessageActivity message, CancellationToken cancellationToken = default(CancellationToken))
        {
            // save this message 

            await this.inner.PostAsync(message, cancellationToken);

            Message = message;
        }

        internal static IMessageActivity RetrieveResponse()
        {
            return Message;
        }
    }
}