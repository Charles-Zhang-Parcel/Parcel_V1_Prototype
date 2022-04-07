using Parcel.Shared.DataTypes;
using Parcel.Shared.Framework;
using Parcel.Shared.Framework.ViewModels;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

namespace Parcel.Toolbox.Finance.Nodes
{
    public class Covariance: ProcessorNode
    {
        #region Node Interface
        public readonly BaseConnector DataTable1Input = new InputConnector(typeof(DataGrid))
        {
            Title = "Data Table 1",
        };
        public readonly BaseConnector ColumnName1Input = new InputConnector(typeof(string))
        {
            Title = "Column Name",
        };
        public readonly BaseConnector DataTable2Input = new InputConnector(typeof(DataGrid))
        {
            Title = "Data Table 2",
        };
        public readonly BaseConnector ColumnName2Input = new InputConnector(typeof(string))
        {
            Title = "Column Name",
        };
        public readonly BaseConnector ValueOutput = new OutputConnector(typeof(double))
        {
            Title = "Value",
        };
        public Covariance()
        {
            Title = NodeTypeName = "Covariance";
            Input.Add(DataTable1Input);
            Input.Add(ColumnName1Input);
            Input.Add(DataTable2Input);
            Input.Add(ColumnName2Input);
            Output.Add(ValueOutput);
        }
        #endregion
        
        #region Processor Interface
        public override OutputConnector MainOutput => ValueOutput as OutputConnector;
        public override NodeExecutionResult Execute()
        {
            DataGrid dataGrid1 = DataTable1Input.FetchInputValue<DataGrid>();
            string columnName1 = ColumnName1Input.FetchInputValue<string>();
            DataGrid dataGrid2 = DataTable2Input.FetchInputValue<DataGrid>();
            string columnName2 = ColumnName2Input.FetchInputValue<string>();
            CovarianceParameter parameter = new CovarianceParameter()
            {
                InputTable1 = dataGrid1,
                InputColumnName1 = columnName1,
                InputTable2 = dataGrid2,
                InputColumnName2 = columnName2
            };
            FinanceHelper.Covariance(parameter);

            ProcessorCache[ValueOutput] = new ConnectorCacheDescriptor(parameter.OutputValue);

            Message.Content = $"Covariance={parameter.OutputValue}";
            Message.Type = NodeMessageType.Normal;
            
            return new NodeExecutionResult(true, null);
        }
        #endregion
    }
}