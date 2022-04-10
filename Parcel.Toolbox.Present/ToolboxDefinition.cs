using System.Reflection;
using Parcel.Shared.Framework;
using Parcel.Toolbox.Present.Nodes;

namespace Parcel.Toolbox.Present
{
    public class ToolboxDefinition: IToolboxEntry
    {
        #region Interface
        public string ToolboxName => "Present";
        public string ToolboxAssemblyFullName => Assembly.GetExecutingAssembly().FullName;
        public ToolboxNodeExport[] ExportNodes => new ToolboxNodeExport[] 
        {
            new ToolboxNodeExport("Present", typeof(PresentOnline)),
        };
        public AutomaticNodeDescriptor[] AutomaticNodes => new AutomaticNodeDescriptor[] { };
        #endregion
    }
}