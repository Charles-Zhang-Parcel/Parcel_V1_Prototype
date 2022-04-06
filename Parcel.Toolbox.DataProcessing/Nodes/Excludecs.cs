using System;
using System.Linq;
using Parcel.Shared.DataTypes;
using Parcel.Shared.Framework;
using Parcel.Shared.Framework.ViewModels;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

namespace Parcel.Toolbox.DataProcessing.Nodes
{
    public class Exclude: ProcessorNode
    {
        #region Node Interface
        public readonly BaseConnector DataTableInput = new InputConnector(typeof(DataGrid))
        {
            Title = "Data Table",
        };
        public readonly BaseConnector ColumnNamesInput = new InputConnector(typeof(string))
        {
            Title = "Column Names",
        };
        public readonly BaseConnector DataTableOutput = new OutputConnector(typeof(DataGrid))
        {
            Title = "Result",
        };
        public Exclude()
        {
            Title = "Exclude";
            Input.Add(DataTableInput);
            Input.Add(ColumnNamesInput);
            Output.Add(DataTableOutput);
        }
        #endregion
        
        #region Processor Interface
        public override OutputConnector MainOutput => DataTableOutput as OutputConnector;
        public override NodeExecutionResult Execute()
        {
            DataGrid dataGrid = DataTableInput.FetchInputValue<DataGrid>();
            string[] columnNames = ColumnNamesInput.FetchInputValue<string>()
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(cn => cn.Trim()).ToArray();
            ExcludeParameter parameter = new ExcludeParameter()
            {
                InputTable = dataGrid,
                InputColumnNames = columnNames,
            };
            DataProcessingHelper.Exclude(parameter);

            ProcessorCache[DataTableOutput] = new ConnectorCacheDescriptor(parameter.OutputTable);

            Message.Content = $"{parameter.OutputTable.Columns.Count} Columns";
            Message.Type = NodeMessageType.Normal;
            
            return new NodeExecutionResult(true, null);
        }
        #endregion
    }
}