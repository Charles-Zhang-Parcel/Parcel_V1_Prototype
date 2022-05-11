using System.Reflection;
using Parcel.Shared.Framework;
using Parcel.Toolbox.Math.Nodes;

namespace Parcel.Toolbox.Math
{
    public class ToolboxDefinition: IToolboxEntry
    {
        #region Interface
        public string ToolboxName => "Math";
        public string ToolboxAssemblyFullName => Assembly.GetExecutingAssembly().FullName;
        public ToolboxNodeExport[] ExportNodes => new[]
        {
            // Basic Numberical Operations
            new ToolboxNodeExport("Add", typeof(Add)),
            new ToolboxNodeExport("Subtract", typeof(Subtract)),
            new ToolboxNodeExport("Multiply", typeof(Multiply)),
            new ToolboxNodeExport("Divide", typeof(Divide)),
            new ToolboxNodeExport("Modulus", typeof(Module)),
            new ToolboxNodeExport("Power", typeof(Power)),
            null, // Divisor line // Math Functions
            new ToolboxNodeExport("Sin", typeof(Sin)),
        };
        public AutomaticNodeDescriptor[] AutomaticNodes => new AutomaticNodeDescriptor[] { };
        #endregion
    }
}