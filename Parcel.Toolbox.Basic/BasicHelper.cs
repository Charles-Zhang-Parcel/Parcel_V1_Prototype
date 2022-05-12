using System;
using System.Collections.Generic;
using System.Reflection;
using Parcel.Shared.Framework;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

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
            
        }
    }
}