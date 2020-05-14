using System;
using System.Runtime.Serialization;

namespace DMC.Invokers.Privates
{
    [Serializable]
    public class NullContainerException : Exception
    {
        public NullContainerException()
        {
        }

        public NullContainerException(string message) : base(message)
        {
        }

        public NullContainerException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NullContainerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}