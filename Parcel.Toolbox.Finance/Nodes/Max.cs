using Parcel.Shared.DataTypes;
using Parcel.Shared.Framework;
using Parcel.Shared.Framework.ViewModels;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

namespace Parcel.Toolbox.Finance.Nodes
{
    public class Max: ProcessorNode
    {
        #region Node Interface
        public readonly BaseConnector DataTableInput = new InputConnector(typeof(DataGrid))
        {
            Title = "Data Table",
        };
        public readonly BaseConnector ColumnNameInput = new InputConnector(typeof(string))
        {
            Title = "Column Name",
        };
        public readonly BaseConnector ValueOutput = new OutputConnector(typeof(double))
        {
            Title = "Value",
        };
        public Max()
        {
            Title = NodeTypeName = "Max";
            Input.Add(DataTableInput);
            Input.Add(ColumnNameInput);
            Output.Add(ValueOutput);
        }
        #endregion
        
        #region Processor Interface
        public override OutputConnector MainOutput => ValueOutput as OutputConnector;
        public override NodeExecutionResult Execute()
        {
            DataGrid dataGrid = DataTableInput.FetchInputValue<DataGrid>();
            string columnName = ColumnNameInput.FetchInputValue<string>();
            MaxParameter parameter = new MaxParameter()
            {
                InputTable = dataGrid,
                InputColumnName = columnName
            };
            FinanceHelper.Max(parameter);

            ProcessorCache[ValueOutput] = new ConnectorCacheDescriptor(parameter.OutputValue);

            Message.Content = $"Max={parameter.OutputValue}";
            Message.Type = NodeMessageType.Normal;
            
            return new NodeExecutionResult(true, null);
        }
        #endregion
    }
}