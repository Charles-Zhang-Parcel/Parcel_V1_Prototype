using System.Reflection;
using Parcel.Shared.Framework;
using Parcel.Toolbox.Graphing.Nodes;

namespace Parcel.Toolbox.Graphing
{
    public class ToolboxDefinition: IToolboxEntry
    {
        #region Interface
        public string ToolboxName => "Graphing";
        public string ToolboxAssemblyFullName => Assembly.GetExecutingAssembly().FullName;
        public ToolboxNodeExport[] ExportNodes => new ToolboxNodeExport[] 
        {
            new ToolboxNodeExport("Line Chart", typeof(LineChart)),
        };
        public AutomaticNodeDescriptor[] AutomaticNodes => new AutomaticNodeDescriptor[] { };
        #endregion
    }
}