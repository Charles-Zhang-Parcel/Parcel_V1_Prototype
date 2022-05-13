using System;
using System.Collections.Generic;
using System.Reflection;
using Parcel.Shared.Framework.ViewModels;

namespace Parcel.Shared.Framework
{
    public interface IProcessor
    {
        public OutputConnector MainOutput { get; }
        public void Evaluate();
        public ConnectorCacheDescriptor this[OutputConnector cacheConnector] { get; }
        public bool HasCache(OutputConnector cacheConnector);
    }

    /// <summary>
    /// Automatic nodes provides a way to quickly define a large library of simple function nodes without defining classes for them
    /// </summary>
    public struct AutomaticNodeDescriptor
    {
        public string NodeName { get; set; }
        public Type[] InputTypes { get; set; }
        public Type[] OutputTypes { get; set; }
        public Func<object[], object[]> CallMarshal { get; set; }

        public AutomaticNodeDescriptor(string nodeName, Type[] inputTypes, Type[] outputTypes, Func<object[], object[]> callMarshal)
        {
            NodeName = nodeName;
            InputTypes = inputTypes;
            OutputTypes = outputTypes;
            CallMarshal = callMarshal;
        }
        public AutomaticNodeDescriptor(string nodeName, Type[] inputTypes, Type outputType, Func<object[], object> callMarshal)
        {
            NodeName = nodeName;
            InputTypes = inputTypes;
            OutputTypes = new []{ outputType };
            CallMarshal = (inputs) => new []{ callMarshal(inputs) };
        }
    }
}