using System;
using System.Collections.Generic;
using System.Text;
using DMC.Device.Business.Invokers.Domains;
using System.Linq;
using DMC.Device.Business.Invokers.Exceptions;
using System.IO;
using System.Text.RegularExpressions;

namespace DMC.Device.Business.Invokers.Implementations
{
    public class GPIOInvokerNotImplemented : IGPIOBusiness
    {

        public string Analog(AnalogGPIOBDM gpio)
        {
            throw new NotImplementedException("Not Implemented.");
        }

        public string Digitial(DigitalGPIOBDM gpio)
        {
            throw new NotImplementedException("Not Implemented.");
        }

    }
}
