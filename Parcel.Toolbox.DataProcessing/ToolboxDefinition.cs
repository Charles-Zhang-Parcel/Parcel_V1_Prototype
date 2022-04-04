using System.Reflection;
using Parcel.Shared.Framework;
using Parcel.Toolbox.DataProcessing.Nodes;

namespace Parcel.Toolbox.DataProcessing
{
    public class ToolboxDefinition: IToolboxEntry
    {
        public string ToolboxName => "Data Processing";
        public string ToolboxAssemblyFullName => Assembly.GetExecutingAssembly().FullName;
        public ToolboxNodeExport[] ExportNodes => new[]
        {
            new ToolboxNodeExport("CSV", typeof(CSV)),
            new ToolboxNodeExport("Data Table", typeof(DataTable)),
            null, // Divisor line
            new ToolboxNodeExport("Extract Columns", typeof(object)),
            new ToolboxNodeExport("Validate Columns", typeof(object)),
            null, // Divisor line
            new ToolboxNodeExport("Convert to Matrix", typeof(object)),
        };
    }
}