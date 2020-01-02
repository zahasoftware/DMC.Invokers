using DMC.Device.Business.Invokers.Domains;
using DMC.Device.Business.Invokers.Exceptions;
using rnet.lib;
using System;

namespace DMC.Device.Business.Invokers.Implementations.Raspberry
{
    public class RaspberryRNetInvoker : IGPIOBusiness, IDisposable
    {
        private readonly IPyProcess rpyProcess;

        public RaspberryRNetInvoker(IPyProcess rpyProcess)
        {
            this.rpyProcess = rpyProcess;
            this.rpyProcess.Start();
        }

        public string Analog(AnalogGPIOBDM gpio)
        {
            try
            {
                rpyProcess.PWM[(int)gpio.Pin].Write(gpio.Value);
                return gpio.Value.ToString();
            }
            catch (Exception nse)
            {
                throw new InvokerException(nse.ToString(), InvokerExceptionType.Error);
            }
        }

        public string Digitial(DigitalGPIOBDM gpio)
        {
            try
            {
                rpyProcess.Switch[(int)gpio.Pin].Write(gpio.Value);
                return gpio.Value == true ? "ON" : "OFF";
            }
            catch (System.Exception nse)
            {
                throw new InvokerException(nse.ToString(), InvokerExceptionType.Error);
            }
        }

        public void Dispose()
        {
            rpyProcess.Exit();
        }




    }
}
