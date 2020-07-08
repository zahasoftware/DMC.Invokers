using NetXP.NetStandard.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DMC.Invokers
{
    public interface IInitializer
    {
        /// <summary>
        /// It is called when start application
        /// </summary>
        void Init();
    }
}
