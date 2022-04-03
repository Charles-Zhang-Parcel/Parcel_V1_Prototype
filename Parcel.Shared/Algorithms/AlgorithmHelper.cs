using System;
using System.Collections.Generic;
using System.Linq;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

namespace Parcel.Shared.Algorithms
{
    public static class AlgorithmHelper
    {
        /// <summary>
        /// Detect cyclic connection
        /// </summary>
        public static bool FindSelf(BaseNode startingNode, BaseNode self)
        {
            if (self == startingNode)
                return true;
            
            if (self is KnotNode knot)
            {
                IEnumerable<BaseNode> outConnections = knot.Next;
                return outConnections.Any(c => FindSelf(startingNode, c));
            }
            else if (self is ProcessorNode processor)
            {
                var outConnections = processor.Output.SelectMany(o => o.Connections)
                    .Where(c => c.Output.IsConnected)
                    .Select(c => c.Output.Node);
                return outConnections.Any(c => FindSelf(startingNode, c));
            }
            else throw new ArgumentException("Invalid node type");
        }
    }
}