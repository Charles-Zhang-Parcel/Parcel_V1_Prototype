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

        #region Interface
        public void DraftTree(IEnumerable<ProcessorNode> targetNodes)
        {
            foreach (ProcessorNode node in targetNodes)
                DraftBranchesForNode(null, node);
        }
        public void ExecuteTree()
        {
            Roots.ForEach(tr => ExecuteTreeNode(tr));
        }
        #endregion

        #region Routines
        private void DraftBranchesForNode(ExecutionTreeNode last, ProcessorNode node)
        {
            if (Traversed.ContainsKey(node))
                return;

            ExecutionTreeNode treeNode = new ExecutionTreeNode(node);
            if(last != null) treeNode.AddChild(last);
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
                        DraftBranchesForNode(treeNode, processor);
                }
            }
        }

        private void ExecuteTreeNode(ExecutionTreeNode node)
        {
            node.Processor.Execute();
            
            foreach (ExecutionTreeNode childNode in node.Children)
                ExecuteTreeNode(childNode);
        }
        #endregion
    }

    public class ExecutionTreeNode
    {
        public ProcessorNode Processor { get; }
        public HashSet<ExecutionTreeNode> Children { get; } = new HashSet<ExecutionTreeNode>();

        public ExecutionTreeNode(ProcessorNode processor)
        {
            Processor = processor;
        }

        public void AddChild(ExecutionTreeNode child)
        {
            Children.Add(child);
        }
    }
}