namespace DMC.Invokers.Attributes
{
    public class InvokerPropertyAttribute : InvokerAttribute
    {
        public InvokerPropertyAttribute(
              string Guid = "",
              string Name = null,
              int Order = 0,
              string Help = null,
              int MaxLength = 255,
              int MinLength = 0,
              string Regex = null)
            : base(Guid, Name, Order, Help)
        {
            this.MaxLength = MaxLength;
            this.MinLength = MinLength;
            this.Regex = Regex;
        }

        public int MaxLength { get; set; }
        public int MinLength { get; set; }
        public string Regex { get; set; }
    }
}
