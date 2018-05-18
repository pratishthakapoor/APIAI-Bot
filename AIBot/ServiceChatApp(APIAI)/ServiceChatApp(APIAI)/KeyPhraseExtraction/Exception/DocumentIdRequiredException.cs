using System;
using System.Runtime.Serialization;

namespace ServiceChatApp_APIAI_
{
    [Serializable]
    internal class DocumentIdRequiredException : Exception
    {
        public DocumentIdRequiredException()
        {
        }

        public DocumentIdRequiredException(string message) : base(message)
        {
        }

        public DocumentIdRequiredException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DocumentIdRequiredException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}