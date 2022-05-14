using System.Collections.Generic;
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
            Title = NodeTypeName = "Graph Stats";
        }
        #endregion
        
        #region Processor Interface
        protected override NodeExecutionResult Execute()
        {
            return new NodeExecutionResult(new NodeMessage(string.Empty), null);
        }
        #endregion

        #region Serialization

        protected override Dictionary<string, NodeSerializationRoutine> ProcessorNodeMemberSerialization { get; } =
            null;
        protected override NodeSerializationRoutine InputConnectorsSerialization { get; } = null;
        #endregion
    }
}