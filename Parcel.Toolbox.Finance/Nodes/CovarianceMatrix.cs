using Parcel.Shared.DataTypes;
using Parcel.Shared.Framework;
using Parcel.Shared.Framework.ViewModels;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

namespace Parcel.Toolbox.Finance.Nodes
{
    /// <remarks>
    /// If wanted, the option for population vs sample (n-1) vs n choice can be exposed as property inside property window,
    /// instead of exposing as pins. To avoid clustering the view. 
    /// </remarks>
    public class CovarianceMatrix: ProcessorNode
    {
        #region Node Interface
        public readonly InputConnector DataTableInput = new InputConnector(typeof(DataGrid))
        {
            Title = "Data Table",
        };
        public readonly OutputConnector DataTableOutput = new OutputConnector(typeof(DataGrid))
        {
            Title = "Result",
        };
        public CovarianceMatrix()
        {
            Title = NodeTypeName = "Covariance Matrix";
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