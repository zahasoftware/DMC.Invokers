using NetXP.NetStandard.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Loader;

namespace DMC.Invokers
{
    public class PluginAssembliesDictionary : Dictionary<string, AssemblyTypesList>
    {
        public IContainer Container;
        public AssemblyLoadContext PluginLoadContext { get; set; }
        public string PluginDir { get; internal set; }
    }
}