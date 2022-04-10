using Parcel.Shared.DataTypes;
using Parcel.Shared.Framework;
using Parcel.Shared.Framework.ViewModels;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

namespace Parcel.Toolbox.Plotting.Nodes
{
    public class Plot: WebPreviewProcessorNode
    {
        #region Node Interface
        public readonly BaseConnector DataTableInput = new InputConnector(typeof(DataGrid))
        {
            Title = "Data Table",
        };
        public readonly BaseConnector DataTableOutput = new OutputConnector(typeof(DataGrid))
        {
            Title = "Value",
        };
        public Plot()
        {
            Title = NodeTypeName = "Plot";
            Input.Add(DataTableInput);
            Output.Add(DataTableOutput);
        }
        #endregion
        
        #region Processor Interface
        public override OutputConnector MainOutput => DataTableOutput as OutputConnector;
        public override NodeExecutionResult Execute()
        {
            DataGrid dataGrid = DataTableInput.FetchInputValue<DataGrid>();

            ProcessorCache[DataTableOutput] = new ConnectorCacheDescriptor(dataGrid);

            Message.Content = $"Plotting...";
            Message.Type = NodeMessageType.Normal;
            
            OpenPreview();
            return new NodeExecutionResult(true, null);
        }
        #endregion
    }
}