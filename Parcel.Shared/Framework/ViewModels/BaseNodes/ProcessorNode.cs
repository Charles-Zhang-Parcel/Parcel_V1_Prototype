using System.Collections.Generic;

namespace Parcel.Shared.Framework.ViewModels.BaseNodes
{
    public abstract class ProcessorNode: BaseNode, IProcessor
    {
        #region Public View Properties
        private string _title;
        public string Title
        {
            get => !string.IsNullOrWhiteSpace(_title) ? _title : _NodeTypeName;
            set => SetField(ref _title, value);
        }
        private string _NodeTypeName;
        public string NodeTypeName
        {
            get => _NodeTypeName;
            set => SetField(ref _NodeTypeName, value);
        }
        
        private string _tooltip;
        public string Tooltip
        {
            get => _tooltip;
            set => SetField(ref _tooltip, value);
        }

        private bool _isPreview;
        public bool IsPreview
        {
            get => _isPreview;
            set => SetField(ref _isPreview, value);
        }

        private NodeMessage _message = new NodeMessage();
        public NodeMessage Message
        {
            get => _message;
            set => SetField(ref _message, value);
        }
        #endregion

        #region Connectors
        public NotifyObservableCollection<BaseConnector> Input { get; } = new NotifyObservableCollection<BaseConnector>();
        public NotifyObservableCollection<BaseConnector> Output { get; } = new NotifyObservableCollection<BaseConnector>();
        #endregion

        #region Interface
        public ProcessorNode()
        {
            Input.WhenAdded(c => c.Node = this)
                .WhenRemoved(c => c.Disconnect());

            Output.WhenAdded(c => c.Node = this)
                .WhenRemoved(c => c.Disconnect());
        }
        public void Disconnect()
        {
            Input.Clear();
            Output.Clear();
        }
        public abstract OutputConnector MainOutput { get; }
        public abstract NodeExecutionResult Execute();

        public Dictionary<BaseConnector, ConnectorCacheDescriptor> ProcessorCache { get; set; } =
            new Dictionary<BaseConnector, ConnectorCacheDescriptor>();

        #endregion
    }
}