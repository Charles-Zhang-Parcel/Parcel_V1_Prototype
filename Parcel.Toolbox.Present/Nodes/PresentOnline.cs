using Parcel.Shared;
using Parcel.Shared.DataTypes;
using Parcel.Shared.Framework;
using Parcel.Shared.Framework.ViewModels;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

namespace Parcel.Toolbox.Present.Nodes
{
    public class PresentOnline: ProcessorNode, IWebPreviewProcessorNode
    {
        #region Node Interface
        public readonly BaseConnector ServerConfigInput = new WebConfigInputConnector()
        {
            Title = "Content",
        };
        public PresentOnline()
        {
            Title = NodeTypeName = "Present";
            Input.Add(ServerConfigInput);
        }
        #endregion
        
        #region Processor Interface
        public override OutputConnector MainOutput => null;
        public override NodeExecutionResult Execute()
        {
            ServerConfig config = ServerConfigInput.FetchInputValue<ServerConfig>();
            WebHostRuntime.Singleton.CurrentLayout = config;
            
            Message.Content = $"Presenting...";
            Message.Type = NodeMessageType.Normal;
            
            ((IWebPreviewProcessorNode)this).OpenPreview("Present");
            return new NodeExecutionResult(true, null);
        }
        #endregion
    }
}