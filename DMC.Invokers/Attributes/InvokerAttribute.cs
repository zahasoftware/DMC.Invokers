using System;
using System.Collections.Generic;
using System.Text;

namespace DMC.Invokers.Attributes
{
    public class InvokerAttribute : Attribute
    {
        /// <summary>
        /// Invoker Attribute, To Configure Interace, It should be obove of the interface definition.
        /// </summary>
        /// <param name="Id">It should be unique between invokers, to update, add  or remove data, you could generate Id here https://www.guidgenerator.com/online-guid-generator.aspx </param>
        /// <param name="Name">Name appears in DMC App</param>
        /// <param name="Order">Order In Plugin List</param>
        /// <param name="Help">Help about how to use it</param>
        /// <param name="OS">List of abailable operative systems.</param>
        public InvokerAttribute(string Id = "", string Name = null, int Order = 0, string Help = null, InvokerOS[] OS = null)
        {
            this.Id = Id;
            this.Name = Name;
            this.Order = Order;
            this.Help = Help;
            this.OS = OS;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public string Help { get; set; }
        public InvokerOS[] OS { get; set; }
    }
}
