using DMC.Invokers.Domains;
using System;
using System.Runtime.Serialization;

namespace DMC.Invokers.Exceptions
{
    [Serializable]
    public class InvokerException : ApplicationException
    {
        public InvokerExceptionType InvokerExceptionType { get; set; }


        public InvokerException(string message) : base(message)
        {
        }

        public InvokerException(string message, InvokerExceptionType invokerExceptionType) : base(message)
        {
            this.InvokerExceptionType = invokerExceptionType;
        }

    }
}