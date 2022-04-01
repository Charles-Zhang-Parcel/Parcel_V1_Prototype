using System;
using System.Reflection;
using Parcel.Shared.Framework;

namespace Parcel.Toolbox.ControlFlow
{
    public class ControlFlowHelper: IToolboxEntry
    {
        public string ToolboxName => "Control Flow";
        public string ToolboxAssemblyFullName => Assembly.GetExecutingAssembly().FullName;
        public ToolboxNodeExport[] ExportNodes => new ToolboxNodeExport[] {};
    }
}