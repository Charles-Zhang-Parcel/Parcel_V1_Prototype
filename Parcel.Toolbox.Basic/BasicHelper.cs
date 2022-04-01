using System;
using System.Reflection;
using Parcel.Shared.Framework;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

namespace Parcel.Toolbox.Basic
{
    public class BasicHelper: IToolboxEntry
    {
        public string ToolboxName => "Basic";
        public string ToolboxAssemblyFullName => Assembly.GetExecutingAssembly().FullName;
        public ToolboxNodeExport[] ExportNodes => new ToolboxNodeExport[]
        {
            new ToolboxNodeExport("Comment", typeof(CommentNode))
        };
    }
}