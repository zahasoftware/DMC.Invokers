using DMC.Invokers.Attributes;
using DMC.Invokers.Domains;
using DMC.Invokers.Exceptions;
using NetXP.NetStandard.DependencyInjection;
using NetXP.NetStandard.Reflection;
using NetXP.NetStandard.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DMC.Invokers
{
    public class InvokerPluginHelper
    {
        private InvokerPluginsDictionary pluginsDictionary;
        private List<Type> invokersInterfacesTypes;
        public string PluginPath { get; set; }
        public Func<IContainer> ContainerFactory { get; set; }

        public InvokerPluginHelper(string pluginPath, Func<IContainer> containerFactory)
        {
            this.pluginsDictionary = new InvokerPluginsDictionary();
            this.PluginPath = pluginPath;
            this.ContainerFactory = containerFactory;
        }



        private Type[] GetInvokersInterfaces()
        {
            if (invokersInterfacesTypes?.Count > 0)
            {
                return invokersInterfacesTypes.ToArray();
            }

            var plugins = this.GetPlugins();

            invokersInterfacesTypes = new List<Type>();

            foreach (var plugin in plugins.Values)
            {
                foreach (var assemblyTypes in plugin.Values)
                {
                    foreach (var type in assemblyTypes)
                    {
                        if (type.Type.IsInterface && type.Type.GetCustomAttributes().Any(o => o.GetType() == typeof(InvokerAttribute)))
                        {
                            invokersInterfacesTypes.Add(type.Type);
                        }
                    }
                }
            }
            return invokersInterfacesTypes.ToArray();
        }

        public IDictionary<string, PluginAssembliesDictionary> GetPlugins()
        {
            if (pluginsDictionary.Count > 0)
            {
                return pluginsDictionary;
            }

            var pluginsDir = Path.Combine(this.PluginPath);

            if (Directory.Exists(pluginsDir))
            {
                var pluginsFolder = Directory.GetDirectories(pluginsDir, "*");

                if (pluginsFolder.Length > 0)
                {
                    foreach (var dir in pluginsFolder)
                    {
                        var dllFiles = Directory.GetFiles(dir, "*.dll");

                        var pluginAssembliesDictioanry = new PluginAssembliesDictionary
                        {
                            container = ContainerFactory?.Invoke()
                        };
                        pluginsDictionary[Path.GetFileName(dir)] = pluginAssembliesDictioanry;

                        foreach (var dllFile in dllFiles)
                        {
                            if (Path.GetFileName(dllFile).Equals("dmc.invokers.dll", StringComparison.OrdinalIgnoreCase))
                            {
                                continue;
                            }

                            var assembly = Assembly.LoadFrom(dllFile);
                            var assemblyTypes = assembly.GetTypes();

                            var pluginTypeList = new AssemblyTypesList() { Assembly = assembly, PluginAssembliesDictionary = pluginAssembliesDictioanry };
                            pluginAssembliesDictioanry[dllFile] = pluginTypeList;
                            foreach (var type in assemblyTypes)
                            {
                                pluginTypeList.Add(
                                  new AssemblyType
                                  {
                                      Type = type,
                                      AssemblyTypesList = pluginTypeList,
                                  });
                            }
                        }

                    }
                }
            }
            return pluginsDictionary;
        }


    }
}
