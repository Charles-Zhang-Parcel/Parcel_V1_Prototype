using System.Reflection;
using Parcel.Shared.Framework;
using Parcel.Toolbox.Finance.Nodes;

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
            new ToolboxNodeExport("Mean", typeof(Mean)),
            new ToolboxNodeExport("Variance", typeof(Variance)),
            new ToolboxNodeExport("Standard Deviation", typeof(StandardDeviation)),
            new ToolboxNodeExport("% Return", typeof(object)),
            new ToolboxNodeExport("Correlation", typeof(object)),
            new ToolboxNodeExport("Covariance", typeof(object)),
            new ToolboxNodeExport("Covariance Matrix", typeof(object)), // This one operates on multiple columns
            new ToolboxNodeExport("Min", typeof(Min)),
            new ToolboxNodeExport("Max", typeof(Max)),
            new ToolboxNodeExport("Sum", typeof(Sum)),
        };
        #endregion
    }
}