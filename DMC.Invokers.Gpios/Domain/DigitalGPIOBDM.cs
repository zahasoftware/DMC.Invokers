using DMC.Device.Business.Invokers.Attributes;

namespace DMC.Device.Business.Invokers.Domains
{
    public class DigitalGPIOBDM
    {
        [InvokerProperty(Id = "p", Name = "p", MaxLength = 50, MinLength = 0)]
        public uint Pin { get; set; }

        [InvokerProperty(Id = "v", Name = "v", MaxLength = 1, MinLength = 0)]
        public bool Value { get; set; }

    }
}