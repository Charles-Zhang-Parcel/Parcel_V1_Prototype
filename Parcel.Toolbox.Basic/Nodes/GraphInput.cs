using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
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
            DefinitionNameChanged = definition =>
            {
                OutputConnector output = Output[Definitions.IndexOf(definition)];
                output.Title = definition.Name;
            };
        }
        #endregion

        #region Routines
        protected override string NewEntryPrefix { get; } = "Input";
        protected override Action<GraphInputOutputDefinition> DefinitionNameChanged { get; }
        protected sealed override void PostAddEntry(GraphInputOutputDefinition definition)
        {
            Output.Add(new OutputConnector(definition.DefaultValue.GetType())
            {
                Title = definition.Name
            });
        }
        protected sealed override void PostRemoveEntry()
        {
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

        #region Serialization
        protected override void DeserializeFinalize()
        {
            foreach (GraphInputOutputDefinition definition in Definitions)
                Output.Add(new OutputConnector(definition.DefaultValue.GetType())
                {
                    Title = definition.Name
                });
        }
        #endregion
    }
}