using System.Collections.ObjectModel;
using System.Linq;
using Parcel.Shared.DataTypes;
using Parcel.Shared.Framework;
using Parcel.Shared.Framework.ViewModels;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

namespace Parcel.Toolbox.DataProcessing.Nodes
{
    public class SQL: ProcessorNode
    {
        #region Node Interface
        public readonly BaseConnector DataTableOutput = new OutputConnector(typeof(DataGrid))
        {
            Title = "Result"
        };
        public readonly BaseConnector ServerConfigOutput = new OutputConnector(typeof(ServerConfig))
        {
            Title = "Server Config"
        };
        public SQL()
        {
            Title = NodeTypeName = "SQL";
            Output.Add(DataTableOutput);
            
            DataTableInputs = new ObservableCollection<InputConnector>()
            {
                new InputConnector(typeof(DataGrid))
                {
                    Title = "Data Table"
                }
            };
            
            AddEntryCommand = new RequeryCommand(
                () => DataTableInputs.Add(new InputConnector(typeof(DataGrid)){ Title = "Data Table" }),
                () => true);
            RemoveEntryCommand = new RequeryCommand(
                () => DataTableInputs.RemoveAt(DataTableInputs.Count - 1),
                () => DataTableInputs.Count > 1);
        }
        #endregion
        
        #region View Binding/Internal Node Properties
        private ObservableCollection<InputConnector> _dataTableInputs;
        public ObservableCollection<InputConnector> DataTableInputs { get => _dataTableInputs; set => SetField(ref _dataTableInputs, value); }
        public IProcessorNodeCommand AddEntryCommand { get; }
        public IProcessorNodeCommand RemoveEntryCommand { get; }
        #endregion
        
        #region Processor Interface
        public override OutputConnector MainOutput => DataTableOutput as OutputConnector;
        public override NodeExecutionResult Execute()
        {
            SQLParameter parameter = new SQLParameter()
            {
                InputTables = DataTableInputs.Select(di => di.FetchInputValue<DataGrid>()).ToArray(),
            };
            DataProcessingHelper.SQL(parameter);

            ProcessorCache[DataTableOutput] = new ConnectorCacheDescriptor(parameter.OutputTable);

            Message.Content = $"{parameter.OutputTable.RowCount} Rows; {parameter.OutputTable.ColumnCount} Columns.";
            Message.Type = NodeMessageType.Normal;
            
            return new NodeExecutionResult(true, null);
        }
        #endregion
    }
}