using System;
using System.Windows;
using Parcel.Shared.Serialization;

namespace Parcel.Shared.Framework.ViewModels.BaseNodes
{
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

        #region Serialization
        internal NodeData Serialize()
        {
            throw new NotImplementedException();
        }

        internal void Deserialize()
        {
            
        }
        public int GetOutputPinID(BaseConnector connector)
        {
            throw new NotImplementedException();
        }
        public int GetInputPinID(BaseConnector connector)
        {
            throw new NotImplementedException();
        }
        public BaseConnector GetOutputPin(int id)
        {
            throw new NotImplementedException();
        }

        public BaseConnector GetInputPin(int id)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}