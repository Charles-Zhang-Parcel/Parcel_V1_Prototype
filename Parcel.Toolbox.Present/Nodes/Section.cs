using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Parcel.Shared;
using Parcel.Shared.DataTypes;
using Parcel.Shared.Framework;
using Parcel.Shared.Framework.ViewModels;
using Parcel.Shared.Framework.ViewModels.BaseNodes;
using Parcel.Shared.Framework.ViewModels.Primitives;

namespace Parcel.Toolbox.Present.Nodes
{
    public class Section: DynamicInputProcessorNode, IWebPreviewProcessorNode
    {
        #region Node Interface
        public readonly PrimitiveStringInputConnector SectionNameInput = new PrimitiveStringInputConnector()
        {
            Title = "Name",
        };
        public readonly OutputConnector ServerConfigOutput = new OutputConnector(typeof(ServerConfig))
        {
            Title = "Config",
        };
        public Section()
        {
            Title = NodeTypeName = "Section";
            Input.Add(SectionNameInput);
            Output.Add(ServerConfigOutput);
            
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
            Input.Add(new WebConfigInputConnector(){ Title = "Content" });
        }
        private void RemoveInputs()
        {
            Input.RemoveAt(Input.Count - 1);
        }
        #endregion
        
        #region Processor Interface
        public override OutputConnector MainOutput => ServerConfigOutput;
        public override NodeExecutionResult Execute()
        {
            string sectionName = SectionNameInput.FetchInputValue<string>();
            ServerConfig newConfig = new ServerConfig()
            {
                Children = Input.Skip(1).Select(i => i.FetchInputValue<ServerConfig>()).ToList(),
                ContentType = CacheDataType.String,
                ObjectContent = sectionName,
                LayoutSpec = LayoutElementType.Section,
            };

            ProcessorCache[ServerConfigOutput] = new ConnectorCacheDescriptor(newConfig);
            Message.Content = $"Presenting...";
            Message.Type = NodeMessageType.Normal;
            
            WebHostRuntime.Singleton.CurrentLayout = newConfig;
            ((IWebPreviewProcessorNode)this).OpenPreview("Present");
            return new NodeExecutionResult(true, null);
        }
        #endregion

        #region Auto-Connect Interface
        public override Tuple<ToolboxNodeExport, Vector, InputConnector>[] AutoGenerateNodes =>
            new Tuple<ToolboxNodeExport, Vector, InputConnector>[]
            {
                new Tuple<ToolboxNodeExport, Vector, InputConnector>(new ToolboxNodeExport("String", typeof(StringNode)), new Vector(-150, -50), SectionNameInput),
            };
        #endregion
    }
}