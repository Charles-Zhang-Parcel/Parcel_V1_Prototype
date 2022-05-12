using System.Linq;
using Parcel.Shared.DataTypes;
using Parcel.Shared.Framework;
using Parcel.Shared.Framework.ViewModels;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

namespace Parcel.Toolbox.DataProcessing.Nodes
{
    public class Append: DynamicInputProcessorNode
    {
        #region Node Interface
        public readonly OutputConnector DataTableOutput = new OutputConnector(typeof(DataGrid))
        {
            Title = "Combined Table",
        };
        public Append()
        {
            Title = NodeTypeName = "Append";
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
            Input.Add(new InputConnector(typeof(DataGrid)){Title = $"Table {Input.Count + 1}"});
        }
        private void RemoveInputs()
        {
            Input.RemoveAt(Input.Count - 1);
        }
        #endregion
        
        #region Processor Interface
        public override OutputConnector MainOutput => DataTableOutput as OutputConnector;
        public override NodeExecutionResult Execute()
        {
            AppendParameter parameter = new AppendParameter()
            {
                InputTables = Input
                    .Where(i => i.Connections.Count != 0)
                    .Select(input => input.FetchInputValue<DataGrid>()).ToArray(),
            };
            DataProcessingHelper.Append(parameter);

            ProcessorCache[DataTableOutput] = new ConnectorCacheDescriptor(parameter.OutputTable);

            Message.Content = $"{parameter.OutputTable.Columns.Count} Columns";
            Message.Type = NodeMessageType.Normal;
            
            return new NodeExecutionResult(true, null);
        }
        #endregion
    }
}