using System.Linq;
using System.Windows;
using Parcel.FrontEnd.NodifyWPF.ViewModels.BaseNodes;

namespace Parcel.FrontEnd.NodifyWPF.ViewModels
{
    public enum ConnectorFlowType
    {
        Input,
        Output
    }

    public enum ConnectorShape
    {
        Circle,
        Triangle,
        Square,
    }

    
    public class BaseConnector: ObservableObject
    {
        private string? _title;
        public string? Title
        {
            get => _title;
            set => SetField(ref _title, value);
        }

        private bool _isConnected;
        public bool IsConnected
        {
            get => _isConnected;
            set => SetField(ref _isConnected, value);
        }

        private Point _anchor;
        public Point Anchor
        {
            get => _anchor;
            set => SetField(ref _anchor, value);
        }

        private BaseNode _node = default!;
        public BaseNode Node
        {
            get => _node;
            internal set
            {
                if (SetField(ref _node, value))
                {
                    OnNodeChanged();
                }
            }
        }

        private ConnectorShape _shape;
        public ConnectorShape Shape
        {
            get => _shape;
            set => SetField(ref _shape, value);
        }

        public ConnectorFlowType FlowType { get; private set; }
        public int MaxConnections { get; set; } = 2;

        public NotifyObservableCollection<BaseConnection> Connections { get; } = new NotifyObservableCollection<BaseConnection>();

        public BaseConnector()
        {
            Connections.WhenAdded(c =>
            {
                c.Input.IsConnected = true;
                c.Output.IsConnected = true;
            }).WhenRemoved(c =>
            {
                if (c.Input.Connections.Count == 0)
                {
                    c.Input.IsConnected = false;
                }

                if (c.Output.Connections.Count == 0)
                {
                    c.Output.IsConnected = false;
                }
            });
        }

        protected virtual void OnNodeChanged()
        {
            if (Node is ProcessorNode processorNode)
            {
                FlowType = processorNode.Input.Contains(this) ? ConnectorFlowType.Input : ConnectorFlowType.Output;
            }
            else if (Node is KnotNode knotNode)
            {
                FlowType = knotNode.Flow;
            }
        }

        public bool IsConnectedTo(BaseConnector connector)
            => Connections.Any(c => c.Input == connector || c.Output == connector);

        public virtual bool AllowsNewConnections()
            => Connections.Count < MaxConnections;

        public void Disconnect()
            => Node.Graph.Schema.DisconnectConnector(this);
    }
}