using Parcel.Shared.DataTypes;
using Parcel.Shared.Framework;
using Parcel.Shared.Framework.ViewModels;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

namespace Parcel.Toolbox.DataProcessing.Nodes
{
    public class Append: ProcessorNode
    {
        #region Node Interface
        public readonly BaseConnector DataTable1Input = new InputConnector(typeof(DataGrid))
        {
            Title = "Data Table 1",
        };
        public readonly BaseConnector DataTable2Input = new InputConnector(typeof(DataGrid))
        {
            Title = "Data Table 2",
        };
        public readonly BaseConnector DataTableOutput = new OutputConnector(typeof(DataGrid))
        {
            Title = "Combined Table",
        };
        public Append()
        {
            Title = "Append";
            Input.Add(DataTable1Input);
            Input.Add(DataTable2Input);
            Output.Add(DataTableOutput);
        }
        #endregion
        
        #region Processor Interface
        public override OutputConnector MainOutput => DataTableOutput as OutputConnector;
        public override NodeExecutionResult Execute()
        {
            DataGrid dataGrid1 = DataTable1Input.FetchInputValue<DataGrid>();
            DataGrid dataGrid2 = DataTable2Input.FetchInputValue<DataGrid>();
            AppendParameter parameter = new AppendParameter()
            {
                InputTable1 = dataGrid1,
                InputTable2 = dataGrid2,
            };
            DataProcessingHelper.Append(parameter);

            ProcessorCache[DataTableOutput] = new ConnectorCacheDescriptor(parameter.OutputTable);

            Message.Content = $"{parameter.OutputTable.Columns.Count} Columns";
            Message.Type = NodeMessageType.Normal;
            
            return new NodeExecutionResult(true, null);
        }
        #endregion
    }
}