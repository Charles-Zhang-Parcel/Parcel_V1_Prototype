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
        }
        #endregion

        #region Routines
        protected sealed override void AddEntry()
        {
            string name = $"Input {Definitions.Count + 1}";
            GraphInputOutputDefinition def = new GraphInputOutputDefinition() {Name = name};
            
            Definitions.Add(def);
            Output.Add(new OutputConnector(def.DefaultValue.GetType())
            {
                Title = name
            });
        }
        protected sealed override void RemoveEntry()
        {
            string name = Definitions[Definitions.Count - 1].Name;
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