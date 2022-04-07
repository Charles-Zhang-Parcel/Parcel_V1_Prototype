using Parcel.Shared.DataTypes;
using Parcel.Shared.Framework;
using Parcel.Shared.Framework.ViewModels;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

namespace Parcel.Toolbox.Finance.Nodes
{
    public class Correlation: ProcessorNode
    {
        #region Node Interface
        public readonly BaseConnector DataTableInput1 = new InputConnector(typeof(DataGrid))
        {
            Title = "Data Table 1",
        };
        public readonly BaseConnector ColumnNameInput1 = new InputConnector(typeof(string))
        {
            Title = "Column Name",
        };
        public readonly BaseConnector DataTableInput2 = new InputConnector(typeof(DataGrid))
        {
            Title = "Data Table 2",
        };
        public readonly BaseConnector ColumnNameInput2 = new InputConnector(typeof(string))
        {
            Title = "Column Name",
        };
        public readonly BaseConnector ValueOutput = new OutputConnector(typeof(double))
        {
            Title = "Value",
        };
        public Correlation()
        {
            Title = NodeTypeName = "Correlation";
            Input.Add(DataTableInput1);
            Input.Add(ColumnNameInput1);
            Input.Add(DataTableInput2);
            Input.Add(ColumnNameInput2);
            Output.Add(ValueOutput);
        }
        #endregion
        
        #region Processor Interface
        public override OutputConnector MainOutput => ValueOutput as OutputConnector;
        public override NodeExecutionResult Execute()
        {
            DataGrid dataGrid1 = DataTableInput1.FetchInputValue<DataGrid>();
            string columnName1 = ColumnNameInput1.FetchInputValue<string>();
            DataGrid dataGrid2 = DataTableInput2.FetchInputValue<DataGrid>();
            string columnName2 = ColumnNameInput2.FetchInputValue<string>();
            CorrelationParameter parameter = new CorrelationParameter()
            {
                InputTable1 = dataGrid1,
                InputColumnName1 = columnName1,
                InputTable2 = dataGrid2,
                InputColumnName2 = columnName2
            };
            FinanceHelper.Correlation(parameter);

            ProcessorCache[ValueOutput] = new ConnectorCacheDescriptor(parameter.OutputValue);

            Message.Content = $"Correlation={parameter.OutputValue}";
            Message.Type = NodeMessageType.Normal;
            
            return new NodeExecutionResult(true, null);
        }
        #endregion
    }
}