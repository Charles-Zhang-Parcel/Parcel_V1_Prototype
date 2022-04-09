using Parcel.Shared.DataTypes;
using Parcel.Shared.Framework;
using Parcel.Shared.Framework.ViewModels;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

namespace Parcel.Toolbox.DataProcessing.Nodes
{
    public class Take: ProcessorNode
    {
        #region Node Interface
        public readonly BaseConnector DataTableInput = new InputConnector(typeof(DataGrid))
        {
            Title = "Data Table",
        };
        public readonly BaseConnector RowCountInput = new InputConnector(typeof(double))
        {
            Title = "Row Count",
        };
        public readonly BaseConnector DataTableOutput = new OutputConnector(typeof(DataGrid))
        {
            Title = "Data Table Column",
        };
        public Take()
        {
            Title = NodeTypeName = "Take";
            Input.Add(DataTableInput);
            Input.Add(RowCountInput);
            Output.Add(DataTableOutput);
        }
        #endregion
        
        #region Processor Interface
        public override OutputConnector MainOutput => DataTableOutput as OutputConnector;
        public override NodeExecutionResult Execute()
        {
            DataGrid dataGrid = DataTableInput.FetchInputValue<DataGrid>();
            double rowCount = RowCountInput.FetchInputValue<double>();
            TakeParameter parameter = new TakeParameter()
            {
                InputTable = dataGrid,
                InputRowCount = (int)rowCount
            };
            DataProcessingHelper.Take(parameter);

            ProcessorCache[DataTableOutput] = new ConnectorCacheDescriptor(parameter.OutputTable);

            Message.Content = $"{parameter.OutputTable.Rows.Count} Rows";
            Message.Type = NodeMessageType.Normal;
            
            return new NodeExecutionResult(true, null);
        }
        #endregion
    }
}