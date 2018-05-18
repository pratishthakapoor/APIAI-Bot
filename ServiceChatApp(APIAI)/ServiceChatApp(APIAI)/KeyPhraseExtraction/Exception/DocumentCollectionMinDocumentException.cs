using System;
using System.Runtime.Serialization;

namespace ServiceChatApp_APIAI_
{
    [Serializable]
    internal class DocumentCollectionMinDocumentException : Exception
    {
        private int v1;
        private int v2;

        public DocumentCollectionMinDocumentException()
        {
        }

        public DocumentCollectionMinDocumentException(string message) : base(message)
        {
        }

        public DocumentCollectionMinDocumentException(int v1, int v2)
        {
            this.v1 = v1;
            this.v2 = v2;
        }

        public DocumentCollectionMinDocumentException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DocumentCollectionMinDocumentException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}