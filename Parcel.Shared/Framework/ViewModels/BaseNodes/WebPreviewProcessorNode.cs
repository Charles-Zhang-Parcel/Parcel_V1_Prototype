namespace Parcel.Shared.Framework.ViewModels.BaseNodes
{
    public abstract class WebPreviewProcessorNode: ProcessorNode
    {
        public abstract override OutputConnector MainOutput { get; }
        public abstract override NodeExecutionResult Execute();

        #region Interface
        public void OpenPreview()
        {
            if (WebHostRuntime.Singleton != null)
            {
                WebHostRuntime.Singleton.LastNode = this;
                WebHostRuntime.Singleton.OpenTarget("Preview");
            }
        }
        #endregion
    }
}