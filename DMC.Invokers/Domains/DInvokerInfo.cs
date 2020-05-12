using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DMC.Invokers.Domains
{
    public class InvokerInfo
    {
        public Type InterfaceType { get; set; }
        public List<DInvokerMethodInfo> Methods { get; set; }
        public string Help { get; set; }
        public string Alias { get; set; }
        public int Order { get; set; }
        public string OS { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public int Version { get; set; }
    }
}
