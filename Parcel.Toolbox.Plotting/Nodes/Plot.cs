using System.Collections.Generic;
using Parcel.Shared.DataTypes;
using Parcel.Shared.Framework;
using Parcel.Shared.Framework.ViewModels;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

namespace Parcel.Toolbox.Plotting.Nodes
{
    public class Plot: ProcessorNode, IWebPreviewProcessorNode
    {
        #region Node Interface
        private readonly InputConnector _dataTableInput = new InputConnector(typeof(DataGrid))
        {
            Title = "Data Table",
        };
        private readonly OutputConnector _dataTableOutput = new OutputConnector(typeof(DataGrid))
        {
            Title = "Value",
        };
        public Plot()
        {
            Title = NodeTypeName = "Plot";
            Input.Add(_dataTableInput);
            Output.Add(_dataTableOutput);
        }
        #endregion
        
        #region Processor Interface
        protected override NodeExecutionResult Execute()
        {
            DataGrid dataGrid = _dataTableInput.FetchInputValue<DataGrid>();

            ((IWebPreviewProcessorNode)this).OpenWebPreview();
            return new NodeExecutionResult(new NodeMessage($"Plotting..."), new Dictionary<OutputConnector, object>()
            {
                {_dataTableOutput, dataGrid}
            });
        }
        #endregion
        
        #region Serialization
        protected override Dictionary<string, NodeSerializationRoutine> ProcessorNodeMemberSerialization { get; } =
            null;
        protected override NodeSerializationRoutine VariantInputConnectorsSerialization { get; } = null;
        #endregion
    }
}