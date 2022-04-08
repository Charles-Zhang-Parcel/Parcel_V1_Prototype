using System.Reflection;
using Parcel.Shared.Framework;

namespace Parcel.Toolbox.DataSource
{
    public class ToolboxDefinition: IToolboxEntry
    {
        #region Interface
        public string ToolboxName => "Data Source";
        public string ToolboxAssemblyFullName => Assembly.GetExecutingAssembly().FullName;
        public ToolboxNodeExport[] ExportNodes => new[]
        {
            // Data Base System
            new ToolboxNodeExport("MS MDL", typeof(object)),
            new ToolboxNodeExport("PL SQL", typeof(object)),
            null, // Divisor line // Web Services
            new ToolboxNodeExport("Yahoo Finance", typeof(object)),
        };
        public AutomaticNodeDescriptor[] AutomaticNodes => new AutomaticNodeDescriptor[] { };
        #endregion
    }
}