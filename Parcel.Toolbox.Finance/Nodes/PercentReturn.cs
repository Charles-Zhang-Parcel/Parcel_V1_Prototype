using Parcel.Shared.DataTypes;
using Parcel.Shared.Framework;
using Parcel.Shared.Framework.ViewModels;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

namespace Parcel.Toolbox.Finance.Nodes
{
    public class PercentReturn: ProcessorNode
    {
        #region Node Interface
        public readonly BaseConnector DataTableInput = new InputConnector(typeof(DataGrid))
        {
            Title = "Data Table",
        };

        public readonly BaseConnector LatestAtTopInput = new InputConnector(typeof(bool))
        {
            Title = "Latest At Top"
        };
        public readonly BaseConnector DataTableOutput = new OutputConnector(typeof(DataGrid))
        {
            Title = "Result",
        };
        public PercentReturn()
        {
            Title = "PercentReturn";
            Input.Add(DataTableInput);
            Input.Add(LatestAtTopInput);
            Output.Add(DataTableOutput);
            
            Tooltip = $"This node takes in a table of time series data in each column; It will automatically trim rows; Non-numerical data will be trimmed.";
            Message.Content = "Input a time series.";
            Message.Type = NodeMessageType.Documentation;
        }
        #endregion
        
        #region Processor Interface
        public override OutputConnector MainOutput => DataTableOutput as OutputConnector;
        public override NodeExecutionResult Execute()
        {
            DataGrid dataGrid = DataTableInput.FetchInputValue<DataGrid>();
            bool latestAtTop = LatestAtTopInput.FetchInputValue<bool>();
            PercentReturnParameter parameter = new PercentReturnParameter()
            {
                InputTable = dataGrid,
                LatestAtTop = latestAtTop
            };
            FinanceHelper.PercentReturn(parameter);

            ProcessorCache[DataTableOutput] = new ConnectorCacheDescriptor(parameter.OutputTable);

            Message.Content = $"{parameter.OutputTable.RowCount} Rows, {parameter.OutputTable.ColumnCount} Columns";
            Message.Type = NodeMessageType.Normal;
            
            return new NodeExecutionResult(true, null);
        }
        #endregion
    }
}