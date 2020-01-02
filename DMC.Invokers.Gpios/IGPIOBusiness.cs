using DMC.Device.Business.Domains;
using DMC.Device.Business.Invokers.Attributes;
using DMC.Device.Business.Invokers.Domains;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMC.Device.Business.Invokers
{
    [Attributes.Invoker(
          Id = "021FE080-CF4C-4A2A-895B-66846F2C28F9"
        , Name = "gpio", Help = "Can control GPIO of a raspberry."
        , OS = new[] { InvokerOS.Raspbian, InvokerOS.Windows })]
    public interface IGPIOBusiness
    {
        [InvokerMethod(Id = "a", Name = "a", Order = 0, Help = "Analog GPIO")]
        string Analog(AnalogGPIOBDM gpio);

        [InvokerMethod(Id = "d", Name = "d", Order = 1, Help = "Digital GPIO")]
        string Digitial(DigitalGPIOBDM gpio);
    }
}
