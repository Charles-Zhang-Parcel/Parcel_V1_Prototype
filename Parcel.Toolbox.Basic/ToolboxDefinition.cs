using System.Reflection;
using Parcel.Shared.Framework;
using Parcel.Shared.Framework.ViewModels.BaseNodes;
using Parcel.Shared.Framework.ViewModels.Primitives;
using Parcel.Toolbox.Basic.Nodes;

namespace Parcel.Toolbox.Basic
{
    public class ToolboxDefinition: IToolboxEntry
    {
        #region Interface
        public string ToolboxName => "Basic";
        public string ToolboxAssemblyFullName => Assembly.GetExecutingAssembly().FullName;
        public ToolboxNodeExport[] ExportNodes => new ToolboxNodeExport[]
        {
            new ToolboxNodeExport("Comment", typeof(CommentNode)),
            new ToolboxNodeExport("Preview", typeof(Preview)),
            null, // Divisor line // Primitive Nodes
            new ToolboxNodeExport("Number", typeof(NumberNode)),
            new ToolboxNodeExport("String", typeof(StringNode)),
            new ToolboxNodeExport("Boolean", typeof(BooleanNode)),
            new ToolboxNodeExport("File", typeof(OpenFileNode)),
            new ToolboxNodeExport("Array", typeof(object)), // Generic array representation of all above types, CANNOT have mixed types
            null, // Divisor line // Queries
            new ToolboxNodeExport("Count", typeof(object)), // Return count of an array
            null, // Divisor line // Basic Operations - Number
            new ToolboxNodeExport("Add", typeof(object)),
            new ToolboxNodeExport("Subtract", typeof(object)),
            new ToolboxNodeExport("Multiply", typeof(object)),
            new ToolboxNodeExport("Divide", typeof(object)),
            new ToolboxNodeExport("Modulus", typeof(object)),
        };
        #endregion
    }
}