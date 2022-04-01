namespace Parcel.FrontEnd.NodifyWPF.ViewModels.BaseNodes
{
    public class KnotNode : BaseNode
    {
        #region View Components
        private BaseConnector _connector = default!;
        public BaseConnector Connector
        {
            get => _connector;
            set
            {
                if (SetField(ref _connector, value))
                {
                    _connector.Node = this;
                }
            }
        }
        #endregion

        public ConnectorFlowType Flow { get; set; }
    }
}