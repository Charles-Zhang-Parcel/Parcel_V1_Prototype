using System;
using System.Collections.Generic;
using System.Reflection;
using Parcel.Shared.Framework.ViewModels;

namespace Parcel.Shared.Framework
{
    public interface IProcessor
    {
        public OutputConnector MainOutput { get; }
        public NodeExecutionResult Execute();
        public Dictionary<OutputConnector, ConnectorCacheDescriptor> ProcessorCache { get; set; }
    }

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