using System;
using System.Collections.Generic;
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
    
    public class SQL: DynamicInputProcessorNode, INodeProperty
    {
        #region Node Interface
        private readonly OutputConnector _dataTableOutput = new OutputConnector(typeof(DataGrid))
        {
            Title = "Result"
        };
        private readonly OutputConnector _serverConfigOutput = new OutputConnector(typeof(ServerConfig))
        {
            Title = "Present"
        };
        public SQL()
        {
            Editors = new List<PropertyEditor>()
            {
                new PropertyEditor("Code", PropertyEditorType.Code, () => _code, o => Code = (string)o)
            };
            
            Title = NodeTypeName = "SQL";
            Output.Add(_dataTableOutput);
            Output.Add(_serverConfigOutput);
            
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
        private string _code = "select * from @Table1";
        public string Code
        {
            get => _code;
            set => SetField(ref _code, value);
        }
        #endregion

        #region Property Editor Interface
        public List<PropertyEditor> Editors { get; }
        #endregion
        
        #region Processor Interface
        public override OutputConnector MainOutput => _dataTableOutput as OutputConnector;

        protected override NodeExecutionResult Execute()
        {
            SQLParameter parameter = new SQLParameter()
            {
                InputTables = Input.Select(i =>
                {
                    DatabaseTableInputConnector connector = i as DatabaseTableInputConnector;
                    var table = connector!.FetchInputValue<DataGrid>();
                    // table.TableName = connector.TableName;
                    return table;
                }).ToArray(),
                InputTableNames = Input.Select(i => (i as DatabaseTableInputConnector)!.TableName).ToArray(),
                InputCommand = Code 
            };
            DataProcessingHelper.SQL(parameter);

            return new NodeExecutionResult(new NodeMessage($"{parameter.OutputTable.RowCount} Rows; {parameter.OutputTable.ColumnCount} Columns."), new Dictionary<OutputConnector, object>()
            {
                {_dataTableOutput, parameter.OutputTable}
            });
        }
        #endregion
    }
}