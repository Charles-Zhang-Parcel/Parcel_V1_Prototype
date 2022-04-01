using System.Reflection;
using Parcel.Shared.Framework;

namespace Parcel.Toolbox.ControlFlow
{
    public class ToolboxDefinition: IToolboxEntry
    {
        #region Interface
        public string ToolboxName => "Control Flow";
        public string ToolboxAssemblyFullName => Assembly.GetExecutingAssembly().FullName;
        public ToolboxNodeExport[] ExportNodes => new ToolboxNodeExport[] {};
        #endregion
    }
}