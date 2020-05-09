using NetXP.NetStandard.DependencyInjection.Implementations.StructureMaps;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMC.Invokers.Privates
{
    public class PluginContainer
    {

        public SMContainer Container { get; }

        public PluginContainer()
        {
            this.Container = new SMContainer(new Container());
        }


        public void Configure(Action<object> c)
        {
            this.Container.Configuration.Configure(c);
        }

        public object Resolve(Type t)
        {
            return this.Container.Resolve(t);
        }

        public void Dispose()
        {
            this.Container.Dispose();
        }

    }
}
