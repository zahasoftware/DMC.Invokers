using System;
using System.Collections.Generic;
using System.Text;

namespace DMC.Invokers.Domains
{
    public class DInvokerMethodParameterInfo
    {
        public string Help { get; set; }
        public string Alias { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public List<DInvokerMethodParameterInfo> PropertiesInfoes { get; set; }
        public int? MaxLength { get; set; }
        public int? MinLength { get; set; }
        public Type Type { get; set; }
        public string Regex { get; set; }
    }
}
