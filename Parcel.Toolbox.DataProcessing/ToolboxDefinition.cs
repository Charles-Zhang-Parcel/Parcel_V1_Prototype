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
            new ToolboxNodeExport("Append", typeof(Append)),
            new ToolboxNodeExport("Extract", typeof(Extract)),
            new ToolboxNodeExport("Exclude", typeof(Exclude)),   // Opposite of Extract
            new ToolboxNodeExport("Validate", typeof(object)),  // Validate and reinterpret formats
            new ToolboxNodeExport("Reinterpret", typeof(object)),  // Validate and reinterpret formats
            new ToolboxNodeExport("Sort", typeof(Sort)),
            new ToolboxNodeExport("Take", typeof(Take)),
            null, // Divisor line // Low Level Operations
            new ToolboxNodeExport("Add", typeof(object)),   // Add cell, add row, add column
            new ToolboxNodeExport("Convert", typeof(object)), // Act on individual columns
            null, // Divisor line // Queries
            new ToolboxNodeExport("Names", typeof(object)), // Return string array of headers
            new ToolboxNodeExport("Size", typeof(object)), // Return integer count of rows and columns
            null, // Divisor line // Data Conversion
            new ToolboxNodeExport("To Matrix", typeof(object)),
        };
    }
}