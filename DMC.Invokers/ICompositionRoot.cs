using NetXP.NetStandard.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMC.Invokers
{
    /// <summary>
    /// Interface registrations
    /// </summary>
    public interface ICompositionRoot
    {
        PluginContext PluginContext { get; set; }
        void ForWindows(IRegister r);
        void ForLinux(IRegister r);
        void ForRaspberry(IRegister r);
    }
}
