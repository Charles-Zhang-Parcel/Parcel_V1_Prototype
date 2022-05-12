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
    public class MatrixMultiply: DynamicInputProcessorNode
    {
        #region Node Interface
        public readonly OutputConnector DataTableOutput = new OutputConnector(typeof(DataGrid))
        {
            Title = "Result"
        };
        public MatrixMultiply()
        {
            Title = NodeTypeName = "Matrix Multiply";
            Output.Add(DataTableOutput);

            AddInputs();
            
            AddEntryCommand = new RequeryCommand(
                () => AddInputs(),
                () => true);
            RemoveEntryCommand = new RequeryCommand(
                () => RemoveInputs(),
                () => Input.Count > 1);
        }
        #endregion

        #region Routines
        private void AddInputs()
        {
            Input.Add(new InputConnector(typeof(DataGrid)){Title = $"Table {Input.Count / 2 + 1}"});
            Input.Add(new PrimitiveBooleanInputConnector() {Title = "Transpose"} );
        }
        private void RemoveInputs()
        {
            Input.RemoveAt(Input.Count - 1);
            Input.RemoveAt(Input.Count - 1);
        }
        #endregion

        #region View Binding/Internal Node Properties
        #endregion

        #region Processor Interface
        public override OutputConnector MainOutput => DataTableOutput as OutputConnector;
        public override NodeExecutionResult Execute()
        {
            MatrixMultiplyParameter parameter = new MatrixMultiplyParameter()
            {
                InputTables = Input
                    .Where((input, index) => index % 2 == 0)
                    .Select(input => input.FetchInputValue<DataGrid>()).ToArray(),
                InputTableShouldTransposes = Input
                    .Where((input, index) => index % 2 == 1)
                    .Select(input => input.FetchInputValue<bool>()).ToArray(),
            };
            DataProcessingHelper.MatrixMultiply(parameter);

            ProcessorCache[DataTableOutput] = new ConnectorCacheDescriptor(parameter.OutputTable);

            Message.Content = $"{parameter.OutputTable.RowCount} Rows; {parameter.OutputTable.ColumnCount} Columns.";
            Message.Type = NodeMessageType.Normal;
            
            return new NodeExecutionResult(true, null);
        }
        #endregion
    }
}