using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Parcel.Shared.Framework;
using Parcel.Shared.Framework.ViewModels;
using Parcel.Shared.Framework.ViewModels.BaseNodes;
using Parcel.Shared.Framework.ViewModels.Primitives;
using Parcel.Shared.Serialization;

namespace Parcel.Toolbox.Basic.Nodes
{
    public class GraphReference: ProcessorNode
    {
        #region Node Interface
        public GraphReference()
        {
            Title = NodeTypeName = "Graph Reference";
        }
        #endregion
        
        #region View Binding/Internal Node Properties
        public string _graphPath = null;
        public string GraphPath
        {
            get => _graphPath;
            set
            {
                SetField(ref _graphPath, value);
                UpdateGraphReference();
            }
        }
        #endregion
        
        #region Routines
        private void UpdateGraphReference()
        {
            if (System.IO.File.Exists(GraphPath))
            {
                CanvasSerialization subgraph = new Subgraph().Load(_graphPath);
                
                Message.Content = $"{subgraph.Nodes.Count} Nodes";
                Message.Type = NodeMessageType.Normal;
            }
        }
        private static Type GetInputNodeType(GraphInputOutputDefinition definition)
        {
            return typeof(StringNode);
        }
        #endregion
        
        #region Processor Interface

        protected override NodeExecutionResult Execute()
        {
            Dictionary<string, object> parameterSet = new Dictionary<string, object>();
            foreach (InputConnector inputConnector in Input)
            {
                parameterSet[inputConnector.Title] = inputConnector.FetchInputValue<object>();
            }
            GraphReferenceParameter parameter = new GraphReferenceParameter()
            {
                InputGraph = GraphPath,
                InputParameterSet = parameterSet
            };
            BasicHelper.GraphReference(parameter);

            Dictionary<OutputConnector, object> cache = new Dictionary<OutputConnector, object>();
            foreach ((string key, object value) in parameter.OutputParameterSet)
            {
                OutputConnector output = Output.SingleOrDefault(o => o.Title == key);
                if (output != null) 
                    cache[output] = value;   
            }

            return new NodeExecutionResult(new NodeMessage($"{parameterSet.Count} Inputs -> {parameter.OutputParameterSet.Count} Outputs"), cache);
        }
        #endregion
        
        #region Auto Generate Interface
        public override Tuple<ToolboxNodeExport, Vector, InputConnector>[] AutoGenerateNodes
        {
            get
            {
                List<Tuple<ToolboxNodeExport, Vector, InputConnector>> auto =
                    new List<Tuple<ToolboxNodeExport, Vector, InputConnector>>();
                
                // Add nodes for additional variable-inputs
                for (int i = 0; i < Input.Count; i++)
                {
                    if(Input[i].Connections.Count != 0) continue;

                    ToolboxNodeExport toolDef = new ToolboxNodeExport(Input[i].Title, GetInputNodeType(null));
                    auto.Add(new Tuple<ToolboxNodeExport, Vector, InputConnector>(toolDef, new Vector(-100, -50 + (i - 1) * 50), Input[i]));
                }
                
                return auto.ToArray();
            }
        }
        public override bool ShouldHaveConnection => Input.Count > 0 && Input.Any(i => i.Connections.Count == 0);
        #endregion
    }
}