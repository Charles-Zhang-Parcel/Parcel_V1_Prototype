namespace Parcel.FrontEnd.NodifyWPF.ViewModels.BaseNodes
{
    public class ProcessorNode: BaseNode
    {
        #region Public View Properties
        private string _title;
        public string Title
        {
            get => _title;
            set => SetField(ref _title, value);
        }
        #endregion

        #region Connectors
        public NodifyObservableCollection<BaseConnector> Input { get; } = new NodifyObservableCollection<BaseConnector>();
        public NodifyObservableCollection<BaseConnector> Output { get; } = new NodifyObservableCollection<BaseConnector>();
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
        #endregion
    }
}