using Parcel.Shared.Framework.ViewModels;

namespace Parcel.Shared.Framework
{
    public enum NodeMessageType
    {
        Empty,
        Normal,
        Error
    }
    
    public class NodeMessage: ObservableObject
    {
        #region Public View Properties
        private NodeMessageType _type;
        public NodeMessageType Type
        {
            get => _type;
            set => SetField(ref _type, value);
        }
        private string _content;
        public string Content
        {
            get => _content;
            set => SetField(ref _content, value);
        }
        #endregion
    }
}