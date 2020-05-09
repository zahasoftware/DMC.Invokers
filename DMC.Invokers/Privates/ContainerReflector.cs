using System;
using System.Collections.Generic;
using System.Text;

namespace DMC.Invokers.Privates
{
    public class ContainerReflector
    {
        private object container;

        public ConfigurationReflector Configuration { get; set; }

        public ContainerReflector()
        {
        }

        public void Init(object container)
        {
            this.container = container;
            this.Configuration = new ConfigurationReflector(this.container);
        }


        public object Resolve(Type interfaceType)
        {
            var containerType = this.container.GetType();
            var resolveMethodInfo = containerType.GetMethod("Resolve");
            var @return = resolveMethodInfo.Invoke(this.container, new[] { interfaceType });

            return @return;
        }

        public void Dispose()
        {
            var containerType = this.container.GetType();
            var resolveMethodInfo = containerType.GetMethod("Dispose");

            resolveMethodInfo.Invoke(this.container, null);
        }
    }
}
