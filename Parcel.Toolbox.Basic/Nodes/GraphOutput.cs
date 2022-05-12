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
    public class GraphOutput: GraphInputOutputNodeBase
    {
        #region Node Interface
        public GraphOutput()
        {
            Title = NodeTypeName = "Graph Output";
            DefinitionNameChanged = definition =>
            {
                InputConnector input = Input[Definitions.IndexOf(definition)];
                input.Title = definition.Name;
            };
        }
        #endregion
        
        #region Routines
        protected override string NewEntryPrefix { get; } = "Output";
        protected override Action<GraphInputOutputDefinition> DefinitionNameChanged { get; }
        protected sealed override void PostAddEntry(GraphInputOutputDefinition definition)
        {
            Input.Add(new InputConnector(definition.DefaultValue.GetType())
            {
                Title = definition.Name
            });
        }
        protected sealed override void PostRemoveEntry()
        {
            Input.RemoveAt(Input.Count - 1);
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

            Message.Content = $"{Definitions.Count} Outputs.";
            Message.Type = NodeMessageType.Normal;
            
            return new NodeExecutionResult(true, null);
        }
        #endregion

        #region Serialization
        protected override void DeserializeFinalize()
        {
            foreach (GraphInputOutputDefinition definition in Definitions)
                Input.Add(new InputConnector(definition.GetType())
                {
                    Title = definition.Name
                });
        }
        #endregion
    }
}