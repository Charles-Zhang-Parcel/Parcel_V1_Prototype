using System;
using System.Collections.Generic;
using System.Reflection;
using Parcel.Shared.Algorithms;
using Parcel.Shared.Framework;
using Parcel.Shared.Framework.ViewModels;
using Parcel.Shared.Framework.ViewModels.BaseNodes;
using Parcel.Shared.Serialization;

namespace Parcel.Toolbox.Basic
{
    #region Parameters
    public class GraphReferenceParameter
    {
        public string InputGraph { get; set; }
        public Dictionary<string, object> InputParameterSet { get; set; }
        public Dictionary<string, object> OutputParameterSet { get; set; }
    }
    #endregion
    
    public static class BasicHelper
    {
        public static void GraphReference(GraphReferenceParameter parameter)
        {
            // Instantiate
            NodesCanvas canvas = new NodesCanvas();
            canvas.Open(parameter.InputGraph);

            // Execute
            parameter.OutputParameterSet = new Subgraph().Execute(canvas, parameter.InputParameterSet);
        }
    }
}