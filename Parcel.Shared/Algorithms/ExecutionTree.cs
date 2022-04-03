using System.Collections.Generic;
using System.Linq;
using Parcel.Shared.Framework.ViewModels.BaseNodes;
using YamlDotNet.Serialization.NodeDeserializers;

namespace Parcel.Shared.Algorithms
{
    public class ExecutionTree
    {
        public List<ExecutionTreeNode> Roots { get; set; } = new List<ExecutionTreeNode>();
        public Dictionary<ProcessorNode, ExecutionTreeNode> Traversed { get; set; } =
            new Dictionary<ProcessorNode, ExecutionTreeNode>();

        public void DraftTree(IEnumerable<ProcessorNode> targetNodes)
        {
            foreach (ProcessorNode node in targetNodes)
                DraftBranchesForNode(node);
        }

        #region Routines
        private void DraftBranchesForNode(ProcessorNode node)
        {
            if (Traversed.ContainsKey(node))
                return;

            ExecutionTreeNode treeNode = new ExecutionTreeNode()
            {
                Processor = node
            };
            Traversed.Add(node, treeNode);
            
            if(!node.Input.Any(i => i.IsConnected))
                Roots.Add(treeNode);
            else
            {
                foreach (BaseNode iter in node.Input.Where(i => i.IsConnected)
                    .Select(i => i.Connections.Single())
                    .Select(c => c.Input.Node))
                {
                    BaseNode input = iter;
                    
                    while (input is KnotNode knot)
                        input = knot.Previous;

                    if(input is ProcessorNode processor)
                        DraftBranchesForNode(processor);
                }
            }
        }
        #endregion
    }

    public class ExecutionTreeNode
    {
        public ProcessorNode Processor { get; set; }
        public ExecutionTreeNode Parent { get; set; }
    }
}