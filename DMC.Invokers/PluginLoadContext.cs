using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace DMC.Invokers
{
    //https://www.strathweb.com/2019/01/collectible-assemblies-in-net-core-3-0/
    public class PluginLoadContext : AssemblyLoadContext
    {

        public PluginLoadContext() : base(isCollectible: true)//isCollectible doesn't appear in netstandard2.1
        {
        }

    }
}
