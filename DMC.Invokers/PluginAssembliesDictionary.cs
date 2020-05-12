using DMC.Invokers.Privates;
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
        public ContainerReflector Container { get; set; }
        public PluginLoadContext PluginLoadContext { get; set; }
        public string PluginDir { get; internal set; }
    }
}