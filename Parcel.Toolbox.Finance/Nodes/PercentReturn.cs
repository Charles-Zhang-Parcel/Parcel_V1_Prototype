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
        public readonly BaseConnector ColumnNameInput = new InputConnector(typeof(string))
        {
            Title = "Date Column Name",
        };
        public readonly BaseConnector DataTableOutput = new OutputConnector(typeof(double))
        {
            Title = "Result",
        };
        public PercentReturn()
        {
            Title = "PercentReturn";
            Input.Add(DataTableInput);
            Input.Add(ColumnNameInput);
            Output.Add(DataTableOutput);
            
            Tooltip = $"This node takes in a table of time series data in each column; It can optionally preserve a column.";
            Message.Content = "Input a time series.";
            Message.Type = NodeMessageType.Documentation;
        }
        #endregion
        
        #region Processor Interface
        public override OutputConnector MainOutput => DataTableOutput as OutputConnector;
        public override NodeExecutionResult Execute()
        {
            DataGrid dataGrid = DataTableInput.FetchInputValue<DataGrid>();
            string columnName = ColumnNameInput.FetchInputValue<string>();
            PercentReturnParameter parameter = new PercentReturnParameter()
            {
                InputTable = dataGrid,
                InputColumnName = columnName,
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