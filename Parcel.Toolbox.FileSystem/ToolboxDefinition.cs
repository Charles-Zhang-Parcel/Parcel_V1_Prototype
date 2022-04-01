using System.Reflection;
using Parcel.Shared.Framework;

namespace Parcel.Toolbox.FileSystem
{
    public class ToolboxDefinition: IToolboxEntry
    {
        #region Interface
        public string ToolboxName => "File System";
        public string ToolboxAssemblyFullName => Assembly.GetExecutingAssembly().FullName;
        public ToolboxNodeExport[] ExportNodes => new ToolboxNodeExport[] {};
        #endregion
    }
}