using System.Windows;

namespace Parcel.FrontEnd.NodifyWPF.ViewModels.BaseNodes
{
    public class BaseNode : ObservableObject
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
    }
}