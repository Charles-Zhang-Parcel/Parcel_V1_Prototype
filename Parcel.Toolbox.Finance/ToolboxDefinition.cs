using System.Reflection;
using Parcel.Shared.Framework;

namespace Parcel.Toolbox.Finance
{
    public class ToolboxDefinition: IToolboxEntry
    {
        #region Interface
        public string ToolboxName => "Finance";
        public string ToolboxAssemblyFullName => Assembly.GetExecutingAssembly().FullName;
        public ToolboxNodeExport[] ExportNodes => new ToolboxNodeExport[] 
        {
            // Basic - Operations on Columns (Those will check and validate column type as Number/Double)
            new ToolboxNodeExport("Mean", typeof(object)),
            new ToolboxNodeExport("Variance", typeof(object)),
            new ToolboxNodeExport("% Return", typeof(object)),
            new ToolboxNodeExport("Correlation", typeof(object)),
            new ToolboxNodeExport("Covariance", typeof(object)),
            new ToolboxNodeExport("Covariance Matrix", typeof(object)), // This one operates on multiple columns
            new ToolboxNodeExport("Min", typeof(object)),
            new ToolboxNodeExport("Max", typeof(object)),
            new ToolboxNodeExport("Sum", typeof(object)),
        };
        #endregion
    }
}