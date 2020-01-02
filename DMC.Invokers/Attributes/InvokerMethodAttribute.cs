using System;
using System.Collections.Generic;
using System.Text;

namespace DMC.Invokers.Attributes
{
    public class InvokerMethodAttribute : InvokerAttribute
    {
        public InvokerMethodAttribute(string Guid = "", string Name = null, int Order = 0, string Help = null) : base(Guid, Name, Order, Help)
        {
        }
    }
}
