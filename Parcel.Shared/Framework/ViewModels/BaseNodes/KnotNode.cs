using System.Collections.Generic;
using System.Linq;

namespace Parcel.Shared.Framework.ViewModels.BaseNodes
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

        #region Accessor
        public BaseNode Previous =>
            Connector.Connections.SingleOrDefault(c => c.Input.Node != this)?.Input.Node ?? null;
        public IEnumerable<BaseNode> Next => Connector.Connections
            .Where(c => c.Input.Node == this || c.Output.IsConnected)
            .Select(c => c.Output.Node);
        #endregion
    }
}