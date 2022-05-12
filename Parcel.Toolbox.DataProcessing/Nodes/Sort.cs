using Parcel.Shared.DataTypes;
using Parcel.Shared.Framework;
using Parcel.Shared.Framework.ViewModels;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

namespace Parcel.Toolbox.DataProcessing.Nodes
{
    public class Sort: ProcessorNode
    {
        #region Node Interface
        public readonly InputConnector DataTableInput = new InputConnector(typeof(DataGrid))
        {
            Title = "Data Table",
        };
        public readonly InputConnector ColumnNameInput = new InputConnector(typeof(string))
        {
            Title = "Column Name",
        };
        public readonly InputConnector ReverseOrderInput = new InputConnector(typeof(bool))
        {
            Title = "Reverse Order",
        };
        public readonly OutputConnector DataTableOutput = new OutputConnector(typeof(DataGrid))
        {
            Title = "Result",
        };
        public Sort()
        {
            Title = NodeTypeName = "Sort";
            Input.Add(DataTableInput);
            Input.Add(ColumnNameInput);
            Input.Add(ReverseOrderInput);
            Output.Add(DataTableOutput);
        }
        #endregion
        
        #region Processor Interface
        public override OutputConnector MainOutput => DataTableOutput as OutputConnector;
        public override NodeExecutionResult Execute()
        {
            DataGrid dataGrid = DataTableInput.FetchInputValue<DataGrid>();
            string columnName = ColumnNameInput.FetchInputValue<string>();
            bool reverseOrder = ReverseOrderInput.FetchInputValue<bool>();
            SortParameter parameter = new SortParameter()
            {
                InputTable = dataGrid,
                InputColumnName = columnName,
                InputReverseOrder = reverseOrder
            };
            DataProcessingHelper.Sort(parameter);

            ProcessorCache[DataTableOutput] = new ConnectorCacheDescriptor(parameter.OutputTable);

            Message.Content = $"{parameter.OutputTable.RowCount} Rows {parameter.OutputTable.ColumnCount} Columns";
            Message.Type = NodeMessageType.Normal;
            
            return new NodeExecutionResult(true, null);
        }
        #endregion
    }
}