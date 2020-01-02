using DMC.Invokers.Domains;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMC.Invokers
{
    public interface IInvoker
    {
        List<InvokerInfo> GetInvokers();
        object Invoke(DInvokerMethodParam param);
    }
}
