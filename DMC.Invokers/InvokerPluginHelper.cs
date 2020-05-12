using DMC.Invokers.Attributes;
using DMC.Invokers.Domains;
using DMC.Invokers.Exceptions;
using NetXP.NetStandard.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using System.Text;

namespace DMC.Invokers
{
    public class InvokerPluginHelper
    {
        protected InvokerPluginsDictionary pluginsDictionary;
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
            return pluginsDictionary;
        }

        public IDictionary<string, PluginAssembliesDictionary> LoadPlugins()
        {
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
            return this.pluginsDictionary;
        }

        public void LoadPlugin(string dir)
        {
            var pluginAssembliesDictioanry = new PluginAssembliesDictionary
            {
                Container = new Privates.ContainerReflector(),
                //PluginLoadContext = new PluginLoadContext(),
                PluginDir = dir
            };

            this.pluginsDictionary[Path.GetFileName(dir)] = pluginAssembliesDictioanry;

            var dllPlugin = Directory.GetFiles(dir, "*.Plugin.dll");
            if (dllPlugin.Count() == 0)
            {
                throw new ApplicationException($"There is not plugin dll in directory {dir}");
            }

            foreach (var dllFile in dllPlugin)
            {
                var pc = new PluginLoadContext(dllFile);
                pluginsDictionary[Path.GetFileName(dir)].PluginLoadContext = pc;
            }

            var dllFiles = Directory.GetFiles(dir, "*.dll");
            foreach (var dllFile in dllFiles)
            {
                var fileName = Path.GetFileName(dllFile);
                if (fileName.Equals("DMC.Invokers.dll") || fileName.Contains(".Plugin.dll"))
                {
                    var assembly = pluginAssembliesDictioanry.PluginLoadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(dllFile)));

                    var pluginTypeList = new AssemblyTypesList() { Assembly = assembly, PluginAssembliesDictionary = pluginAssembliesDictioanry };
                    pluginAssembliesDictioanry[dllFile] = pluginTypeList;
                }
            }

            foreach (var pa in pluginAssembliesDictioanry)
            {
                var assemblyTypes = pa.Value.Assembly.GetTypes();
                foreach (var type in assemblyTypes)
                {
                    var pad = pa.Value;
                    pad.Add(new AssemblyType
                    {
                        Type = type,
                        AssemblyTypesList = pa.Value,
                    });
                }
            }
        }

    }
}
