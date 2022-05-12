using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using Parcel.Shared.DataTypes;
using Parcel.Shared.Framework;
using Parcel.Shared.Framework.ViewModels;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

namespace Parcel.Toolbox.Basic.Nodes
{
    public class GraphInput: GraphInputOutputNodeBase
    {
        #region Node Interface
        public GraphInput()
        {
            Title = NodeTypeName = "Graph Input";
        }
        #endregion

        #region Routines
        protected sealed override void AddEntry()
        {
            Definitions.Add(new GraphInputOutputDefinition() {Name = $"Input {Definitions.Count + 1}"});
            Output.Add(new OutputConnector(typeof(DataGrid))
            {
                Title = $"Input {Definitions.Count}"
            });
        }
        protected sealed override void RemoveEntry()
        {
            Definitions.RemoveAt(Definitions.Count - 1);
            Output.RemoveAt(Input.Count - 1);
        }
        #endregion
        
        #region Processor Interface
        public override NodeExecutionResult Execute()
        {
            for (int index = 0; index < Definitions.Count; index++)
            {
                GraphInputOutputDefinition definition = Definitions[index];
                ProcessorCache[Output[index]] = new ConnectorCacheDescriptor(definition.DefaultValue);
            }

            Message.Content = $"{Definitions.Count} Inputs.";
            Message.Type = NodeMessageType.Normal;
            
            return new NodeExecutionResult(true, null);
        }
        #endregion
    }
}