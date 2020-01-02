using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DMC.Invokers.Domains
{
    public class DInvokerMethodInfo
    {
        //public MethodInfo MethodInfo { get; set; }
        public string Help { get; set; }
        public string Alias { get; set; }
        public int Order { get; set; }
        public string ReturnType { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public List<DInvokerMethodParameterInfo> ParameterInfoes { get; set; }
    }
}
