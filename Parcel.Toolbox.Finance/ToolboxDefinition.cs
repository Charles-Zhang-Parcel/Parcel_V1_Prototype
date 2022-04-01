using System.Reflection;
using Parcel.Shared.Framework;

namespace Parcel.Toolbox.Finance
{
    public class ToolboxDefinition: IToolboxEntry
    {
        #region Interface
        public string ToolboxName => "Finance";
        public string ToolboxAssemblyFullName => Assembly.GetExecutingAssembly().FullName;
        public ToolboxNodeExport[] ExportNodes => new ToolboxNodeExport[] {};
        #endregion
    }
}