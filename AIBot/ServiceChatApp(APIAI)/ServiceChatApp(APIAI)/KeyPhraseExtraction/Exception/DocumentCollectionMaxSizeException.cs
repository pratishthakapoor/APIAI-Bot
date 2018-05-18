using System;
using System.Runtime.Serialization;

namespace ServiceChatApp_APIAI_
{
    [Serializable]
    internal class DocumentCollectionMaxSizeException : Exception
    {
        private int collectionSize;
        private int v;

        public DocumentCollectionMaxSizeException()
        {
        }

        public DocumentCollectionMaxSizeException(string message) : base(message)
        {
        }

        public DocumentCollectionMaxSizeException(int collectionSize, int v)
        {
            this.collectionSize = collectionSize;
            this.v = v;
        }

        public DocumentCollectionMaxSizeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DocumentCollectionMaxSizeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}