using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Parcel.FrontEnd.NodifyWPF.ViewModels
{
    public class GraphSchema
    {
        #region Add Connection
        public bool CanAddConnection(BaseConnector source, object target)
        {
            if (target is BaseConnector con)
            {
                return source != con
                    && source.Node != con.Node
                    && source.Node.Graph == con.Node.Graph
                    && source.Shape == con.Shape
                    && source.AllowsNewConnections()
                    && con.AllowsNewConnections()
                    && (source.FlowType != con.FlowType || con.Node is KnotNode)
                    && !source.IsConnectedTo(con);
            }
            else if (source.AllowsNewConnections() && target is ProcessorNode node)
            {
                var allConnectors = source.FlowType == ConnectorFlowType.Input ? node.Output : node.Input;
                return allConnectors.Any(c => c.AllowsNewConnections());
            }

            return false;
        }

        public bool TryAddConnection(BaseConnector source, object? target)
        {
            if (target != null && CanAddConnection(source, target))
            {
                if (target is BaseConnector connector)
                {
                    AddConnection(source, connector);
                    return true;
                }
                else if (target is ProcessorNode node)
                {
                    AddConnection(source, node);
                    return true;
                }
            }

            return false;
        }

        private void AddConnection(BaseConnector source, BaseConnector target)
        {
            var sourceIsInput = source.FlowType == ConnectorFlowType.Input;

            source.Node.Graph.Connections.Add(new BaseConnection()
            {
                Input = sourceIsInput ? source : target,
                Output = sourceIsInput ? target : source
            });
        }

        private void AddConnection(BaseConnector source, ProcessorNode target)
        {
            var allConnectors = source.FlowType == ConnectorFlowType.Input ? target.Output : target.Input;
            var connector = allConnectors.First(c => c.AllowsNewConnections());

            AddConnection(source, connector);
        }

        #endregion

        public void DisconnectConnector(BaseConnector connector)
        {
            var graph = connector.Node.Graph;
            var connections = connector.Connections.ToList();
            connections.ForEach(c => graph.Connections.Remove(c));
        }

        public void SplitConnection(BaseConnection connection, Point location)
        {
            var connector = connection.Output;

            var knot = new KnotNode()
            {
                Location = location,
                Flow = connector.FlowType,
                Connector = new BaseConnector
                {
                    MaxConnections = connection.Output.MaxConnections + connection.Input.MaxConnections,
                    Shape = connection.Input.Shape
                }
            };
            connection.Graph.Nodes.Add(knot);

            AddConnection(connector, knot.Connector);
            AddConnection(knot.Connector, connection.Input);

            connection.Remove();
        }

        public void AddCommentAroundNodes(IList<BaseNode> nodes, string? text = default)
        {
            var rect = nodes.GetBoundingBox(50);
            var comment = new CommentNode()
            {
                Location = rect.Location,
                Size = rect.Size,
                Title = text ?? "New comment"
            };

            nodes[0].Graph.Nodes.Add(comment);
        }
    }
}
