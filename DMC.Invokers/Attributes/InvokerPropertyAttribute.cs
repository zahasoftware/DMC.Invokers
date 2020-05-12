namespace DMC.Invokers.Attributes
{
    /// <summary>
    /// Apply custom name, order and restriction to properties of plugin
    /// </summary>
    public class InvokerPropertyAttribute : InvokerAttribute
    {
        /// <summary>
        /// Apply custom name, order and restriction to properties of plugin
        /// </summary>
        /// <param name="Id">Allow add edit or remove an attribute, you should not edit this property after plugin is loaded</param>
        /// <param name="Name">Custom name of propertie.</param>
        /// <param name="Order">Order of the propertie show in plugin</param>
        /// <param name="Help">Help about to use this property</param>
        /// <param name="MaxLength">Restriction of property depending of type (To string type affect the lenght and numbers type the value)</param>
        /// <param name="MinLength">Restriction of property depending of type (To string type affect the lenght and numbers type the value)</param>
        /// <param name="Regex">Allow restrictions to property</param>
        public InvokerPropertyAttribute(
              string Id = "",
              string Name = null,
              int Order = 0,
              string Help = null,
              int MaxLength = 255,
              int MinLength = 0,
              string Regex = null)
            : base(Id, Name, Order, Help)
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
