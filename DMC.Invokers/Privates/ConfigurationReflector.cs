using System;

namespace DMC.Invokers.Privates
{
    public class ConfigurationReflector
    {
        private object container;

        public ConfigurationReflector(object container)
        {
            this.container = container;
        }

        public void Configure(Action<object> c)
        {
            var contaierType = this.container.GetType();
            var configureMethodInfo = contaierType.GetMethod("Configure");
            configureMethodInfo.Invoke(this.container, new[] { c });
        }
    }
}