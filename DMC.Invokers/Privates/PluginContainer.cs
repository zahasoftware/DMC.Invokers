using Unity;
using System;
using System.Collections.Generic;
using System.Text;
using NetXP.NetStandard.DependencyInjection.Implementations.UnityDI;

namespace DMC.Invokers.Privates
{
    public class PluginContainer
    {

        public UContainer Container { get; set; }

        public PluginContainer()
        {
            this.Container = new UContainer(new Unity.UnityContainer());
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
