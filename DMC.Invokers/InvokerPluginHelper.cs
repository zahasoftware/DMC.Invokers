using DMC.Invokers.Attributes;
using DMC.Invokers.Domains;
using DMC.Invokers.Exceptions;
using NetXP.NetStandard.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
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

            var pluginsDir = this.PluginPath;

            if (Directory.Exists(pluginsDir))
            {
                var pluginsFolder = Directory.GetDirectories(pluginsDir, "*");

                if (pluginsFolder.Length > 0)
                {
                    foreach (var dir in pluginsFolder)
                    {
                        LoadPlugin(dir);
                    }
                }
            }
            return pluginsDictionary;
        }

        public void LoadPlugin(string dir)
        {
            var pluginAssembliesDictioanry = new PluginAssembliesDictionary
            {
                Container = new Privates.ContainerReflector(),
                PluginLoadContext = new PluginLoadContext(),
                PluginDir = dir
            };
            pluginsDictionary[Path.GetFileName(dir)] = pluginAssembliesDictioanry;

            var dllFiles = Directory.GetFiles(dir, "*.dll");
            foreach (var dllFile in dllFiles)
            {

                Assembly assembly = null;
                //assembly = Assembly.LoadFrom(dllFile);
                //assembly = pluginsDictionary[Path.GetFileName(dir)].PluginLoadContext.LoadFromAssemblyPath(dllFile);
                using (var fs = File.Open(dllFile, FileMode.Open))
                {
                    assembly = pluginsDictionary[Path.GetFileName(dir)].PluginLoadContext.LoadFromStream(fs);
                }

                var pluginTypeList = new AssemblyTypesList() { Assembly = assembly, PluginAssembliesDictionary = pluginAssembliesDictioanry };

                pluginAssembliesDictioanry[dllFile] = pluginTypeList;

            }

            foreach (var pa in pluginAssembliesDictioanry)
            {
                var assemblyTypes = pa.Value.Assembly.GetTypes();
                foreach (var type in assemblyTypes)
                {
                    var pad = pa.Value;
                    pad.Add(
                                          new AssemblyType
                                          {
                                              Type = type,
                                              AssemblyTypesList = pa.Value,
                                          });
                }
            }
        }

    }
}
