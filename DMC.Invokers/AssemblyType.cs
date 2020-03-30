using System;

namespace DMC.Invokers
{
    public class AssemblyType
    {
        public Type Type { get; set; }
        public AssemblyTypesList AssemblyTypesList { get; internal set; }

        public override string ToString()
        {
            return this.Type.ToString();
        }
    }
}