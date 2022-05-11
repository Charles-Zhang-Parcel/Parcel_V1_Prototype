using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Parcel.Shared.Framework.ViewModels;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

namespace Parcel.Shared.Serialization
{
    internal class GraphSerializer
    {
        #region Interface
        public void Serialize(string filePath, CanvasSerialization canvas)
        {
            // Book-keeping structures
            Dictionary<BaseNode, NodeData> nodeMapping = new Dictionary<BaseNode, NodeData>();

            // Serialize nodes
            List<NodeData> nodes = canvas.Nodes.Select(n =>
            {
                NodeData serialized = n.Serialize();
                nodeMapping[n] = serialized;
                return serialized;
            }).ToList();
            
            // Serialize connections
            List<ConnectionData> connections = canvas.Connections.Select(c =>
            {
                return new ConnectionData()
                {
                    Source = nodeMapping[c.Input.Node],
                    SourcePin = c.Input.Node.GetOutputPinID(c.Input),
                    Destination = nodeMapping[c.Output.Node],
                    DestinationPin = c.Output.Node.GetInputPinID(c.Output)
                };
            }).ToList();
            
            // Serialize
            NodeGraph graph = new NodeGraph()
            {
                Nodes = nodes,
                Connections = connections
            };
            Stream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
            new BinaryFormatter().Serialize(stream, graph);  
            stream.Close();
        }
        public CanvasSerialization Deserialize(string filePath, NodesCanvas canvas)
        {
            // Load raw graph data
            Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);  
            NodeGraph graph = (NodeGraph) new BinaryFormatter().Deserialize(stream);  
            stream.Close();
            
            // Book-keeping structures
            Dictionary<NodeData, BaseNode> nodeMapping = new Dictionary<NodeData, BaseNode>();
            
            // Deserialize nodes
            List<BaseNode> nodes = graph.Nodes.Select(n =>
            {
                BaseNode deserialized = n.Deserialize();
                nodeMapping[n] = deserialized;
                return deserialized;
            }).ToList();
            // Deserialize connections
            List<BaseConnection> connections = graph.Connections.Select(c =>
            {
                return new BaseConnection()
                {
                    Graph = canvas,
                    Input = nodeMapping[c.Source].GetOutputPin(c.SourcePin),
                    Output = nodeMapping[c.Destination].GetInputPin(c.DestinationPin)
                };
            }).ToList();
            
            // Reconstruct canvas
            CanvasSerialization loaded = new CanvasSerialization()
            {
                Nodes = nodes,
                Connections = connections
            };
            return loaded;
        }
        #endregion
    }
}