﻿using System.Collections.Generic;
using System.Linq;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

namespace Parcel.Shared.Algorithms
{
    public class ExecutionQueue: IExecutionGraph
    {
        #region Internal State
        private List<ProcessorNode> Queue { get; set; } = new List<ProcessorNode>();
        #endregion
        
        #region Interface
        public void InitializeGraph(IEnumerable<ProcessorNode> targetNodes)
        {
            foreach (ProcessorNode processorNode in targetNodes)
                UpdateNodePosition(null, processorNode);
        }
        public void ExecuteGraph()
            => Queue.ForEach(n => n.Execute());
        #endregion

        #region Routines
        private void UpdateNodePosition(ProcessorNode last, ProcessorNode node)
        {
            if(!node.Input.Any(i => i.IsConnected))
                Queue.Insert(0, node);
            else
            {
                if (!Queue.Contains(node))
                {
                    if (last == null)
                        Queue.Add(node);
                    else
                        Queue.Insert(Queue.IndexOf(last), node);
                }
                else if (last != null)
                {
                    Queue.Remove(node);
                    Queue.Insert(Queue.IndexOf(last), node);
                }
                
                foreach (BaseNode iter in node.Input.Where(i => i.IsConnected)
                    .Select(i => i.Connections.Single())
                    .Select(c => c.Input.Node))
                {
                    BaseNode input = iter;
                    
                    while (input is KnotNode knot)
                        input = knot.Previous;

                    if(input is ProcessorNode processor)
                        UpdateNodePosition(node, processor);
                }
            }
        }
        #endregion
    }
}