using System.Collections.Generic;
using Parcel.Shared.Framework.ViewModels;

namespace Parcel.Shared.Framework
{
    public readonly struct NodeExecutionResult
    {
        #region Accesor
        public bool Success => Message.Type != NodeMessageType.Error;
        public string ErrorMessage => Message.Type == NodeMessageType.Error ? Message.Content : null;
        #endregion

        #region Construction
        public NodeExecutionResult(NodeMessage message, Dictionary<OutputConnector, object> caches)
        {
            Message = message;
            Caches = caches;
        }
        #endregion

        #region States
        public Dictionary<OutputConnector, object> Caches { get; }
        public NodeMessage Message { get; }
        #endregion
    }
}