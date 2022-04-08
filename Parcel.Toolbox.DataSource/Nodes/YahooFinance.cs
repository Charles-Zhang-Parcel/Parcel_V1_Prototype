using System;
using System.Windows;
using Parcel.Shared.DataTypes;
using Parcel.Shared.Framework;
using Parcel.Shared.Framework.ViewModels;
using Parcel.Shared.Framework.ViewModels.BaseNodes;
using Parcel.Shared.Framework.ViewModels.Primitives;

namespace Parcel.Toolbox.DataSource.Nodes
{
    public class YahooFinance: ProcessorNode
    {
        #region Node Interface
        public readonly InputConnector SymbolInput = new PrimitiveStringInputConnector()
        {
            Title = "Symbol",
        };
        public readonly  InputConnector StartDateInput = new PrimitiveDateTimeInputConnector()
        {
            Title = "Start Date"
        };
        public readonly  InputConnector EndDateInput = new PrimitiveDateTimeInputConnector()
        {
            Title = "End Date"
        };
        public readonly InputConnector IntervalInput = new PrimitiveStringInputConnector()
        {
            Title = "Internal",
        };
        public readonly OutputConnector DataTableOutput = new OutputConnector(typeof(DataGrid))
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

        #region Auto Connect Interface
        public override bool ShouldGenerateConnection => SymbolInput.Connections.Count == 0;
        public override Tuple<ToolboxNodeExport, Vector, InputConnector>[] AutoGenerateNodes =>
            new Tuple<ToolboxNodeExport, Vector, InputConnector>[]
            {
                new Tuple<ToolboxNodeExport, Vector, InputConnector>(new ToolboxNodeExport("String", typeof(StringNode)), new Vector(-250, -100), SymbolInput),
                new Tuple<ToolboxNodeExport, Vector, InputConnector>(new ToolboxNodeExport("Start Date", typeof(DateTimeNode)), new Vector(-250, -50), SymbolInput),
                new Tuple<ToolboxNodeExport, Vector, InputConnector>(new ToolboxNodeExport("End Date", typeof(DateTimeNode)), new Vector(-250, 0), SymbolInput),
                new Tuple<ToolboxNodeExport, Vector, InputConnector>(new ToolboxNodeExport("Interval", typeof(StringNode)), new Vector(-250, 50), SymbolInput)
            };
        #endregion
    }
}