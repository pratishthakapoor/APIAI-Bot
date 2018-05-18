using System;
using System.Runtime.Serialization;

namespace ServiceChatApp_APIAI_
{
    [Serializable]
    internal class DocumentMaxSizeException : Exception
    {
        private string id;
        private int size;
        private int v;

        public DocumentMaxSizeException()
        {
        }

        public DocumentMaxSizeException(string message) : base(message)
        {
        }

        public DocumentMaxSizeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public DocumentMaxSizeException(string id, int size, int v)
        {
            this.id = id;
            this.size = size;
            this.v = v;
        }

        protected DocumentMaxSizeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}