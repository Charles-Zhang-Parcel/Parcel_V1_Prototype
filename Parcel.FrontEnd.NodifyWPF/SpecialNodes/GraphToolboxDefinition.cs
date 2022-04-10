using System.Reflection;
using Parcel.Shared.Framework;
using Parcel.Toolbox.Finance.Nodes;

namespace Parcel.FrontEnd.NodifyWPF.SpecialNodes
{
    public class GraphToolboxDefinition: IToolboxEntry
    {
        #region Interface
        public string ToolboxName => "Special";
        public string ToolboxAssemblyFullName => Assembly.GetExecutingAssembly().FullName;
        public ToolboxNodeExport[] ExportNodes => new ToolboxNodeExport[] 
        {
            // Special - Specialized Graph Visualization
            new ToolboxNodeExport("Graph Stats", typeof(GraphStats)),
            new ToolboxNodeExport("Console Output", typeof(object)), // With options to specify how many lines to show
            new ToolboxNodeExport("Python Snippet", typeof(object)), // With auto binding inputs and outputs
            new ToolboxNodeExport("Host Address", typeof(HostAddress)),
        };
        public AutomaticNodeDescriptor[] AutomaticNodes => new AutomaticNodeDescriptor[] { };
        #endregion
    }
}