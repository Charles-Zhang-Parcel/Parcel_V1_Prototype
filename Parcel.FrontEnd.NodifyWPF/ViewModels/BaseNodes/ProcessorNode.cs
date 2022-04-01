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
        #endregion
    }
}