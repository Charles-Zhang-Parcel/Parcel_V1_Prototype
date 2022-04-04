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
            // Data Types and IO
            new ToolboxNodeExport("CSV", typeof(CSV)),
            new ToolboxNodeExport("Data Table", typeof(DataTable)),
            null, // Divisor line // High Level Operations
            new ToolboxNodeExport("Append", typeof(object)),
            new ToolboxNodeExport("Extract", typeof(object)),
            new ToolboxNodeExport("Validate", typeof(object)),  // Validate and reinterpret formats
            new ToolboxNodeExport("Reinterpret", typeof(object)),  // Validate and reinterpret formats
            new ToolboxNodeExport("Sort", typeof(object)),
            null, // Divisor line // Low Level Operations
            new ToolboxNodeExport("Add", typeof(object)),
            null, // Divisor line // Data Conversion
            new ToolboxNodeExport("To Matrix", typeof(object)),
        };
    }
}