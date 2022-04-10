using System.Collections.Generic;
using System.Linq;
using Parcel.Shared;
using Parcel.Shared.DataTypes;
using Parcel.Shared.Framework;
using Parcel.Shared.Framework.ViewModels;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

namespace Parcel.Toolbox.Present.Nodes
{
    public class PresentOnline: DynamicInputProcessorNode, IWebPreviewProcessorNode
    {
        #region Node Interface
        public readonly PrimitiveStringInputConnector PresentNameInput = new PrimitiveStringInputConnector()
        {
            Title = "Name",
        };
        public PresentOnline()
        {
            Title = NodeTypeName = "Present";
            Input.Add(PresentNameInput);
            
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
        public override OutputConnector MainOutput => null;
        public override NodeExecutionResult Execute()
        {
            ServerConfig config = new ServerConfig()
            {
                Children = Input.Skip(1).Select(i => i.FetchInputValue<ServerConfig>()).ToList(),
                LayoutSpec = LayoutElementType.Presentation
            };
            
            Message.Content = $"Presenting...";
            Message.Type = NodeMessageType.Normal;
            
            WebHostRuntime.Singleton.CurrentLayout = config;
            ((IWebPreviewProcessorNode)this).OpenPreview("Present");
            return new NodeExecutionResult(true, null);
        }
        #endregion
    }
}