using Parcel.Shared.DataTypes;
using Parcel.Shared.Framework;
using Parcel.Shared.Framework.ViewModels;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

namespace Parcel.Toolbox.Finance.Nodes
{
    public class CovarianceMatrix: ProcessorNode
    {
        #region Node Interface
        public readonly BaseConnector DataTableInput = new InputConnector(typeof(DataGrid))
        {
            Title = "Data Table",
        };
        public readonly BaseConnector DataTableOutput = new OutputConnector(typeof(DataGrid))
        {
            Title = "Result",
        };
        public CovarianceMatrix()
        {
            Title = NodeTypeName = "Variance";
            Input.Add(DataTableInput);
            Output.Add(DataTableOutput);
        }
        #endregion
        
        #region Processor Interface
        public override OutputConnector MainOutput => DataTableOutput as OutputConnector;
        public override NodeExecutionResult Execute()
        {
            DataGrid dataGrid = DataTableInput.FetchInputValue<DataGrid>();
            CovarianceMatrixParameter parameter = new CovarianceMatrixParameter()
            {
                InputTable = dataGrid
            };
            FinanceHelper.CovarianceMatrix(parameter);

            ProcessorCache[DataTableOutput] = new ConnectorCacheDescriptor(parameter.OutputTable);

            Message.Content = $"{parameter.OutputTable.Columns.Count} Rows {parameter.OutputTable.Columns[0].Length} Columns";
            Message.Type = NodeMessageType.Normal;
            
            return new NodeExecutionResult(true, null);
        }
        #endregion
    }
}