using System.Collections.Generic;
using Parcel.Shared.Framework.ViewModels;

namespace Parcel.Shared.Framework
{
    public interface IProcessor
    {
        public OutputConnector MainOutput { get; }
        public NodeExecutionResult Execute();
        public Dictionary<BaseConnector, ConnectorCacheDescriptor> ProcessorCache { get; set; }
    }
}