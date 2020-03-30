using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DMC.Invokers.Domains
{
    public class DInvokerMethodParam
    {
        public string Interface { get; set; }
        public string Method { get; set; }
        public DInvokerMethodParamValue[] Params { get; set; }
        public string InvokerId { get; set; }
    }
}
