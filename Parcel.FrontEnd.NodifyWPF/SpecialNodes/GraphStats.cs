using Parcel.Shared.DataTypes;
using Parcel.Shared.Framework;
using Parcel.Shared.Framework.ViewModels;
using Parcel.Shared.Framework.ViewModels.BaseNodes;
using Parcel.Toolbox.Finance;

namespace Parcel.FrontEnd.NodifyWPF.SpecialNodes
{
    /// <summary>
    /// The property panel of this can have special referencing to Graph-level setting
    /// </summary>
    public class GraphStats: ProcessorNode
    {
        #region Node Interface
        public GraphStats()
        {
            Title = "Graph Stats";
        }
        #endregion
        
        #region Processor Interface
        public override OutputConnector MainOutput => null;
        public override NodeExecutionResult Execute()
        {
            Message.Content = string.Empty;
            Message.Type = NodeMessageType.Normal;
            
            return new NodeExecutionResult(true, null);
        }
        #endregion
    }
}