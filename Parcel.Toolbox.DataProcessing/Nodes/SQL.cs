using System;
using System.Collections.ObjectModel;
using System.Linq;
using Parcel.Shared.DataTypes;
using Parcel.Shared.Framework;
using Parcel.Shared.Framework.ViewModels;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

namespace Parcel.Toolbox.DataProcessing.Nodes
{
    public class DatabaseTableInputConnector: InputConnector
    {
        #region Properties
        private string _tableName = "New Key";
        public string TableName
        {
            get => _tableName;
            set => SetField(ref _tableName, value);
        }
        #endregion

        public DatabaseTableInputConnector(string tableName) : base(typeof(DataGrid))
        {
            TableName = tableName;
            Title = "Data Table";
        }
    }
    
    public class SQL: ProcessorNode
    {
        #region Node Interface
        public readonly BaseConnector DataTableOutput = new OutputConnector(typeof(DataGrid))
        {
            Title = "Result"
        };
        public readonly BaseConnector ServerConfigOutput = new OutputConnector(typeof(ServerConfig))
        {
            Title = "Present"
        };
        public SQL()
        {
            Title = NodeTypeName = "SQL";
            Output.Add(DataTableOutput);
            
            Input.Add(new DatabaseTableInputConnector("Table 1"));
            
            AddEntryCommand = new RequeryCommand(
                () => Input.Add(new DatabaseTableInputConnector($"Table {Input.Count + 1}")),
                () => true);
            RemoveEntryCommand = new RequeryCommand(
                () => Input.RemoveAt(Input.Count - 1),
                () => Input.Count > 1);
        }
        #endregion

        #region View Binding/Internal Node Properties
        public IProcessorNodeCommand AddEntryCommand { get; }
        public IProcessorNodeCommand RemoveEntryCommand { get; }
        #endregion
        
        #region Processor Interface
        public override OutputConnector MainOutput => DataTableOutput as OutputConnector;
        public override NodeExecutionResult Execute()
        {
            SQLParameter parameter = new SQLParameter()
            {
                InputTables = Input.Select(i => i.FetchInputValue<DataGrid>()).ToArray(),
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