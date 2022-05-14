using System;
using System.Collections.Generic;
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
        private readonly InputConnector _symbolInput = new PrimitiveStringInputConnector()
        {
            Title = "Symbol",
        };
        private readonly  InputConnector _startDateInput = new PrimitiveDateTimeInputConnector()
        {
            Title = "Start Date"
        };
        private readonly  InputConnector _endDateInput = new PrimitiveDateTimeInputConnector()
        {
            Title = "End Date"
        };
        private readonly InputConnector _intervalInput = new PrimitiveStringInputConnector()
        {
            Title = "Interval",
        };
        private readonly OutputConnector _dataTableOutput = new OutputConnector(typeof(DataGrid))
        {
            Title = "Data Table"
        }; 
        public YahooFinance()
        {
            Title = NodeTypeName = "YahooFinance";
            Input.Add(_symbolInput);
            Input.Add(_startDateInput);
            Input.Add(_endDateInput);
            Input.Add(_intervalInput);
            Output.Add(_dataTableOutput);
        }
        #endregion
        
        #region Processor Interface
        protected override NodeExecutionResult Execute()
        {
            YahooFinanceParameter parameter = new YahooFinanceParameter()
            {
                InputSymbol = _symbolInput.FetchInputValue<string>(),
                InputInterval = _intervalInput.FetchInputValue<string>(),
                InputStartDate = _startDateInput.FetchInputValue<DateTime>(),
                InputEndDate = _endDateInput.FetchInputValue<DateTime>()
            };
            DataSourceHelper.YahooFinance(parameter);

            return new NodeExecutionResult(new NodeMessage($"{parameter.OutputTable.RowCount} Rows; {parameter.OutputTable.ColumnCount} Columns."), new Dictionary<OutputConnector, object>()
            {
                {_dataTableOutput, parameter.OutputTable}
            });
        }
        #endregion
        
        #region Serialization
        protected override Dictionary<string, NodeSerializationRoutine> ProcessorNodeMemberSerialization { get; } =
            null;
        protected override NodeSerializationRoutine InputConnectorsSerialization { get; } = null;
        #endregion

        #region Auto Connect Interface
        public override bool ShouldHaveConnection => _symbolInput.Connections.Count == 0;
        public override Tuple<ToolboxNodeExport, Vector, InputConnector>[] AutoGenerateNodes =>
            new Tuple<ToolboxNodeExport, Vector, InputConnector>[]
            {
                new Tuple<ToolboxNodeExport, Vector, InputConnector>(new ToolboxNodeExport("String", typeof(StringNode)), new Vector(-250, -100), _symbolInput),
                new Tuple<ToolboxNodeExport, Vector, InputConnector>(new ToolboxNodeExport("Start Date", typeof(DateTimeNode)), new Vector(-250, -50), _startDateInput),
                new Tuple<ToolboxNodeExport, Vector, InputConnector>(new ToolboxNodeExport("End Date", typeof(DateTimeNode)), new Vector(-250, 0), _endDateInput),
                new Tuple<ToolboxNodeExport, Vector, InputConnector>(new ToolboxNodeExport("Interval", typeof(StringNode)), new Vector(-250, 50), _intervalInput)
            };
        #endregion
    }
}