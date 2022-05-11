using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Parcel.Shared.Serialization;

namespace Parcel.Shared.Framework.ViewModels.BaseNodes
{
    public class NodeSerializationRoutine
    {
        public string Key { get; set; }
        public Func<object> Serialize { get; set; }
        public Action<object> Deserialize { get; set; }

        public NodeSerializationRoutine(string key, Func<object> serialize, Action<object> deserialize)
        {
            Key = key;
            Serialize = serialize;
            Deserialize = deserialize;
        }
    }
    
    public abstract class BaseNode : ObservableObject
    {
        #region Public View Properties
        private Point _location;
        public Point Location
        {
            get => _location;
            set => SetField(ref _location, value);
        }
        #endregion

        #region References
        private NodesCanvas _graph = default!;
        public NodesCanvas Graph
        {
            get => _graph;
            internal set => SetField(ref _graph, value);
        }
        #endregion

        #region Serialization Interface
        public abstract List<NodeSerializationRoutine> MemberSerialization { get; set; }
        #endregion

        #region Serialization
        internal NodeData Serialize()
        {
            return new NodeData()
            {
                NodeType = this.GetType(),
                NodeMembers = MemberSerialization.ToDictionary(ms => ms.Key, ms => ms.Serialize())
            };
        }

        internal void Deserialize(Dictionary<string, object> members)
        {
            Dictionary<string, NodeSerializationRoutine> temp = MemberSerialization.ToDictionary(ms => ms.Key, ms => ms);
            foreach ((string key, object data) in members)
            {
                temp[key].Deserialize(data);
            }
        }

        public abstract int GetOutputPinID(BaseConnector connector);
        public abstract int GetInputPinID(BaseConnector connector);
        public abstract BaseConnector GetOutputPin(int id);
        public abstract BaseConnector GetInputPin(int id);
        #endregion
    }
}