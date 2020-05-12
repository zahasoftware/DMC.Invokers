using System;
using System.Collections.Generic;
using System.Text;

namespace DMC.Invokers.Attributes
{
    /// <summary>
    /// Allows the interface to appear as a plugin, 
    /// </summary>
    public class InvokerMethodAttribute : InvokerAttribute
    {
        /// <summary>
        ///  Allow to call this method from plugin GUI or Terminal
        /// </summary>
        /// <param name="Id">See base class to view this comment</param>
        /// <param name="Name">See base class to view this comment</param>
        /// <param name="Order">See base class to view this comment</param>
        /// <param name="Help">See base class to view this comment</param>
        public InvokerMethodAttribute(string Id = "", string Name = null, int Order = 0, string Help = null) : base(Id, Name, Order, Help)
        {
        }
    }
}
