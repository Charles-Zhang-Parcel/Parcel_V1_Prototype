using System;
using System.Windows;
using Parcel.Shared.DataTypes;
using Parcel.Shared.Framework;
using Parcel.Shared.Framework.ViewModels;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

namespace Parcel.Toolbox.DataProcessing.Nodes
{
    public class CSV: ProcessorNode
    {
        #region Node Interface
        public readonly BaseConnector PathInput = new PrimitiveStringInputConnector()
        {
            Title = "Path",
        };
        public readonly  BaseConnector HeaderInput = new PrimitiveBooleanInputConnector()
        {
            Title = "Contains Header"
        };
        public readonly BaseConnector DataTableOutput = new OutputConnector(typeof(DataGrid))
        {
            Title = "Data Table"
        }; 
        public CSV()
        {
            Title = NodeTypeName = "CSV";
            Input.Add(PathInput);
            Input.Add(HeaderInput);
            Output.Add(DataTableOutput);
        }
        #endregion
        
        #region Processor Interface
        public override OutputConnector MainOutput => DataTableOutput as OutputConnector;
        public override NodeExecutionResult Execute()
        {
            CSVParameter parameter = new CSVParameter()
            {
                InputPath = PathInput.FetchInputValue<string>(),
                InputContainsHeader = HeaderInput.FetchInputValue<bool>()
            };
            DataProcessingHelper.CSV(parameter);

            ProcessorCache[DataTableOutput] = new ConnectorCacheDescriptor(parameter.OutputTable);

            Message.Content = $"{parameter.OutputTable.RowCount} Rows; {parameter.OutputTable.ColumnCount} Columns.";
            Message.Type = NodeMessageType.Normal;
            
            return new NodeExecutionResult(true, null);
        }
        #endregion
    }
}