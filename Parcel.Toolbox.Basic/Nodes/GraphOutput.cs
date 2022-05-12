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
        }
        #endregion
        
        #region Routines
        protected sealed override void AddEntry()
        {
            var name = $"Output {Definitions.Count + 1}";
            var def = new GraphInputOutputDefinition() {Name = name};
            
            Definitions.Add(def);
            Input.Add(new InputConnector(def.DefaultValue.GetType())
            {
                Title = name
            });
        }
        protected sealed override void RemoveEntry()
        {
            Definitions.RemoveAt(Definitions.Count - 1);
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