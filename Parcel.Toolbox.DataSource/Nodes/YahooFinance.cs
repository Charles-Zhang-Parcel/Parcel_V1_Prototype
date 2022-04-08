using System;
using Parcel.Shared.DataTypes;
using Parcel.Shared.Framework;
using Parcel.Shared.Framework.ViewModels;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

namespace Parcel.Toolbox.DataSource.Nodes
{
    public class YahooFinance: ProcessorNode
    {
        #region Node Interface
        public readonly BaseConnector SymbolInput = new PrimitiveStringInputConnector()
        {
            Title = "Symbol",
        };
        public readonly  BaseConnector StartDateInput = new PrimitiveDateTimeInputConnector()
        {
            Title = "Start Date"
        };
        public readonly  BaseConnector EndDateInput = new PrimitiveDateTimeInputConnector()
        {
            Title = "End Date"
        };
        public readonly BaseConnector IntervalInput = new PrimitiveStringInputConnector()
        {
            Title = "Internal",
        };
        public readonly BaseConnector DataTableOutput = new OutputConnector(typeof(DataGrid))
        {
            Title = "Data Table"
        }; 
        public YahooFinance()
        {
            Title = NodeTypeName = "YahooFinance";
            Input.Add(SymbolInput);
            Input.Add(StartDateInput);
            Input.Add(EndDateInput);
            Input.Add(IntervalInput);
            Output.Add(DataTableOutput);
        }
        #endregion
        
        #region Processor Interface
        public override OutputConnector MainOutput => DataTableOutput as OutputConnector;
        public override NodeExecutionResult Execute()
        {
            YahooFinanceParameter parameter = new YahooFinanceParameter()
            {
                InputSymbol = SymbolInput.FetchInputValue<string>(),
                InputInterval = IntervalInput.FetchInputValue<string>(),
                InputStartDate = StartDateInput.FetchInputValue<DateTime>(),
                InputEndDate = EndDateInput.FetchInputValue<DateTime>()
            };
            DataSourceHelper.YahooFinance(parameter);

            ProcessorCache[DataTableOutput] = new ConnectorCacheDescriptor(parameter.OutputTable);

            Message.Content = $"{parameter.OutputTable.RowCount} Rows; {parameter.OutputTable.ColumnCount} Columns.";
            Message.Type = NodeMessageType.Normal;
            
            return new NodeExecutionResult(true, null);
        }
        #endregion
    }
}