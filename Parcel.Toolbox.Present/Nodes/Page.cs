using System;
using System.Collections.Generic;
using System.Windows;
using Parcel.Shared;
using Parcel.Shared.DataTypes;
using Parcel.Shared.Framework;
using Parcel.Shared.Framework.ViewModels;
using Parcel.Shared.Framework.ViewModels.BaseNodes;
using Parcel.Shared.Framework.ViewModels.Primitives;

namespace Parcel.Toolbox.Present.Nodes
{
    public class Page: ProcessorNode, IWebPreviewProcessorNode
    {
        #region Node Interface
        public readonly BaseConnector ServerConfigInput = new WebConfigInputConnector()
        {
            Title = "Content",
        };
        public readonly PrimitiveStringInputConnector PageNameInput = new PrimitiveStringInputConnector()
        {
            Title = "Name",
        };
        public readonly OutputConnector ServerConfigOutput = new OutputConnector(typeof(ServerConfig))
        {
            Title = "Config",
        };
        public Page()
        {
            Title = NodeTypeName = "Page";
            Input.Add(PageNameInput);
            Input.Add(ServerConfigInput);
            Output.Add(ServerConfigOutput);
        }
        #endregion
        
        #region Processor Interface
        public override OutputConnector MainOutput => ServerConfigOutput;
        public override NodeExecutionResult Execute()
        {
            ServerConfig incomeConfig = ServerConfigInput.FetchInputValue<ServerConfig>();
            string pageName = PageNameInput.FetchInputValue<string>();
            ServerConfig newConfig = new ServerConfig()
            {
                Children = new List<ServerConfig>() {incomeConfig},
                ContentType = CacheDataType.String,
                ObjectContent = pageName,
                LayoutSpec = LayoutElementType.Page,
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
                new Tuple<ToolboxNodeExport, Vector, InputConnector>(new ToolboxNodeExport("String", typeof(StringNode)), new Vector(-150, -50), PageNameInput),
            };
        #endregion
    }
}