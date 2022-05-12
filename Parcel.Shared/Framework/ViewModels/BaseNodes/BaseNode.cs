using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Parcel.Shared.Serialization;

namespace Parcel.Shared.Framework.ViewModels.BaseNodes
{
    public class NodeSerializationRoutine
    {
        public Func<object> Serialize { get; set; }
        public Action<object> Deserialize { get; set; }

        public NodeSerializationRoutine(Func<object> serialize, Action<object> deserialize)
        {
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
        public abstract Dictionary<string, NodeSerializationRoutine> MemberSerialization { get; }
        #endregion

        #region Serialization
        internal NodeData Serialize()
        {
            // Instance members
            Dictionary<string, object> members = MemberSerialization.ToDictionary(ms => ms.Key, ms => ms.Value.Serialize());
            // Base members
            members[nameof(Location)] = _location;

            return new NodeData()
            {
                NodeType = this.GetType().AssemblyQualifiedName,
                NodeMembers = members 
            };
        }

        internal void Deserialize(Dictionary<string, object> members, NodesCanvas canvas)
        {
            // Base members
            this.Graph = canvas;
            _location = (Point)members[nameof(Location)];
            
            // Instance members
            Dictionary<string, NodeSerializationRoutine> instanceMembers = MemberSerialization;
            foreach ((string key, object data) in members)
            {
                if(instanceMembers.ContainsKey(key))
                    instanceMembers[key].Deserialize(data);
            }
        }

        public abstract int GetOutputPinID(OutputConnector connector);
        public abstract int GetInputPinID(InputConnector connector);
        public abstract BaseConnector GetOutputPin(int id);
        public abstract BaseConnector GetInputPin(int id);
        #endregion
    }
}