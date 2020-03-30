using NetXP.NetStandard.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace DMC.Invokers
{
    public class PluginAssembliesDictionary : Dictionary<string, AssemblyTypesList>
    {
        public IContainer container;

    }
}