using System.Collections.Generic;
using System.Reflection;

namespace DMC.Invokers
{
    public class AssemblyTypesList : List<AssemblyType>
    {
        public PluginAssembliesDictionary PluginAssembliesDictionary { get; internal set; }
        public Assembly Assembly { get; set; }
    }
}