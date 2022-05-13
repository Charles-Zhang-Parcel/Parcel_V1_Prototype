using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Parcel.Shared.DataTypes;
using Parcel.Shared.Framework;
using Parcel.Shared.Framework.ViewModels;
using Parcel.Shared.Framework.ViewModels.BaseNodes;
using Parcel.Shared.Framework.ViewModels.Primitives;

namespace Parcel.Toolbox.DataProcessing.Nodes
{
    public class Extract: DynamicInputProcessorNode
    {
        #region Node Interface
        private readonly InputConnector _dataTableInput = new InputConnector(typeof(DataGrid))
        {
            Title = "Data Table",
        };
        private readonly OutputConnector _dataTableOutput = new OutputConnector(typeof(DataGrid))
        {
            Title = "Result",
        };
        public Extract()
        {
            Title = NodeTypeName = "Extract";
            Input.Add(_dataTableInput);
            Output.Add(_dataTableOutput);
            
            AddInputs();
            
            AddEntryCommand = new RequeryCommand(
                AddInputs,
                () => true);
            RemoveEntryCommand = new RequeryCommand(
                RemoveInputs,
                () => Input.Count > 2);
        }
        #endregion
        
        #region Routines
        private void AddInputs()
        {
            Input.Add(new PrimitiveStringInputConnector() {Title = "Column Name"});
        }
        private void RemoveInputs()
        {
            Input.RemoveAt(Input.Count - 1);
        }
        #endregion
        
        #region Processor Interface
        protected override NodeExecutionResult Execute()
        {
            DataGrid dataGrid = _dataTableInput.FetchInputValue<DataGrid>();
            ExtractParameter parameter = new ExtractParameter()
            {
                InputTable = dataGrid,
                InputColumnNames = Input.Skip(1)
                    .Select(input => input.FetchInputValue<string>()).ToArray(),
            };
            DataProcessingHelper.Extract(parameter);

            return new NodeExecutionResult(new NodeMessage($"{parameter.OutputTable.RowCount} Rows; {parameter.OutputTable.ColumnCount} Columns"), new Dictionary<OutputConnector, object>()
            {
                {_dataTableOutput, parameter.OutputTable}
            });
        }
        #endregion

        #region Auto Generate Interface
        public override Tuple<ToolboxNodeExport, Vector, InputConnector>[] AutoGenerateNodes
        {
            get
            {
                List<Tuple<ToolboxNodeExport, Vector, InputConnector>> auto =
                    new List<Tuple<ToolboxNodeExport, Vector, InputConnector>>();
                for (int i = 1; i < Input.Count; i++)
                {
                    if(Input[i].Connections.Count != 0) continue;

                    ToolboxNodeExport toolDef = new ToolboxNodeExport("Column Name", typeof(StringNode));
                    auto.Add(new Tuple<ToolboxNodeExport, Vector, InputConnector>(toolDef, new Vector(-100, -50 + (i - 1) * 50), Input[i] as InputConnector));
                }
                return auto.ToArray();
            }
        }
        public override bool ShouldHaveConnection => _dataTableInput.Connections.Count == 0 ||
                                                     (Input.Count > 1 && Input.Skip(1).Any(i => i.Connections.Count == 0));
        #endregion
    }
}