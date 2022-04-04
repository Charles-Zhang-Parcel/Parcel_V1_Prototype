using System.Collections.Generic;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

namespace Parcel.Shared.Algorithms
{
    public interface IExecutionGraph
    {
        public void InitializeGraph(IEnumerable<ProcessorNode> targetNodes);
        public void ExecuteGraph();
    }
}