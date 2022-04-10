using Parcel.Shared.Framework.ViewModels;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

namespace Parcel.Shared.Framework
{
    public interface IWebPreviewProcessorNode
    {
        #region Interface
        void OpenPreview(string target = "Preview")
        {
            if (WebHostRuntime.Singleton != null && this is ProcessorNode processorNode)
            {
                WebHostRuntime.Singleton.LastNode = processorNode;
                WebHostRuntime.Singleton.OpenTarget(target);
            }
        }
        #endregion
    }
}