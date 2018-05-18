using System;
using System.Runtime.Serialization;

namespace ServiceChatApp_APIAI_
{
    [Serializable]
    internal class DocumentMinSizeException : Exception
    {
        private string id;
        private int size;
        private int v;

        public DocumentMinSizeException()
        {
        }

        public DocumentMinSizeException(string message) : base(message)
        {
        }

        public DocumentMinSizeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public DocumentMinSizeException(string id, int size, int v)
        {
            this.id = id;
            this.size = size;
            this.v = v;
        }

        protected DocumentMinSizeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}