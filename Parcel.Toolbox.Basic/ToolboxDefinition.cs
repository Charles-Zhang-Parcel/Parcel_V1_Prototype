using System.Reflection;
using Parcel.Shared.Framework;
using Parcel.Shared.Framework.ViewModels.BaseNodes;
using Parcel.Shared.Framework.ViewModels.Primitives;

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
            null, // Divisor line
            new ToolboxNodeExport("Number", typeof(NumberNode)),
            new ToolboxNodeExport("String", typeof(StringNode)),
            new ToolboxNodeExport("Boolean", typeof(BooleanNode)),
            new ToolboxNodeExport("File", typeof(OpenFileNode)),
        };
        #endregion
    }
}