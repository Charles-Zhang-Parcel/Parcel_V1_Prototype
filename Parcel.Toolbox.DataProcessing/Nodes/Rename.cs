using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Parcel.Shared.DataTypes;
using Parcel.Shared.Framework;
using Parcel.Shared.Framework.ViewModels;
using Parcel.Shared.Framework.ViewModels.BaseNodes;
using Parcel.Shared.Framework.ViewModels.Primitives;

namespace Parcel.Toolbox.DataProcessing.Nodes
{
    public class Rename: DynamicInputProcessorNode
    {
        #region Node Interface
        public readonly InputConnector DataTableInput = new InputConnector(typeof(DataGrid))
        {
            Title = "Original Table"
        };
        public readonly BaseConnector DataTableOutput = new OutputConnector(typeof(DataGrid))
        {
            Title = "Data Table"
        };
        public Rename()
        {
            Title = NodeTypeName = "Rename";
            Input.Add(DataTableInput);
            Output.Add(DataTableOutput);

            AddInputs();
            
            AddEntryCommand = new RequeryCommand(
                () => AddInputs(),
                () => true);
            RemoveEntryCommand = new RequeryCommand(
                () => RemoveInputs(),
                () => Input.Count > 2);
        }
        #endregion

        #region Routines
        private void AddInputs()
        {
            Input.Add(new PrimitiveStringInputConnector() {Title = "Column"});
            Input.Add(new PrimitiveStringInputConnector() {Title = "New Name"} );
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
            RenameParameter parameter = new RenameParameter()
            {
                InputTable = DataTableInput.FetchInputValue<DataGrid>(),
                InputColumns = Input.Skip(1)
                    .Where((input, index) => index % 2 == 0)
                    .Select(input => input.FetchInputValue<string>()).ToArray(),
                InputColumnNewNames = Input.Skip(1)
                    .Where((input, index) => index % 2 == 1)
                    .Select(input => input.FetchInputValue<string>()).ToArray(),
            };
            DataProcessingHelper.Rename(parameter);

            ProcessorCache[DataTableOutput] = new ConnectorCacheDescriptor(parameter.OutputTable);

            Message.Content = $"{(Input.Count - 1) / 2} columns renamed.";
            Message.Type = NodeMessageType.Normal;
            
            return new NodeExecutionResult(true, null);
        }
        #endregion

        #region Auto-Connect Interface
        public override Tuple<ToolboxNodeExport, Vector, InputConnector>[] AutoGenerateNodes
        {
            get
            {
                List<Tuple<ToolboxNodeExport, Vector, InputConnector>> auto =
                    new List<Tuple<ToolboxNodeExport, Vector, InputConnector>>();
                for (int i = 1; i < Input.Count; i+=2)
                {
                    if(Input[i].Connections.Count != 0) continue;

                    var toolDef = new ToolboxNodeExport("Input Name", typeof(StringNode));
                    auto.Add(new Tuple<ToolboxNodeExport, Vector, InputConnector>(toolDef, new Vector(-100, -50 + (i - 1) * 50), Input[i] as InputConnector));
                    auto.Add(new Tuple<ToolboxNodeExport, Vector, InputConnector>(toolDef, new Vector(-100, (i - 1) * 50), Input[i+1] as InputConnector));
                }
                return auto.ToArray();
            }
        }
        public override bool ShouldHaveConnection => DataTableInput.Connections.Count == 0 ||
                                                     (Input.Count > 1 && Input.Skip(1).Any(i => i.Connections.Count == 0));
        #endregion
    }
}