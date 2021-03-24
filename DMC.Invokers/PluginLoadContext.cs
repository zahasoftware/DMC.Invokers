using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace DMC.Invokers
{
    //https://www.strathweb.com/2019/01/collectible-assemblies-in-net-core-3-0/
    public class PluginLoadContext : AssemblyLoadContext
    {
        private AssemblyDependencyResolver _resolver;

        public PluginLoadContext(string pluginPath) : base(isCollectible: true)
        {
            _resolver = new AssemblyDependencyResolver(pluginPath);
        }


        protected override Assembly Load(AssemblyName assemblyName)
        {
            string assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
            if (assemblyPath != null)
            {
                MemoryStream ms = new MemoryStream();
                using (var fs = File.Open(assemblyPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    fs.CopyTo(ms);
                }
                ms.Position = 0;
                return LoadFromStream(ms);
            }

            return null;
        }

    }
}
