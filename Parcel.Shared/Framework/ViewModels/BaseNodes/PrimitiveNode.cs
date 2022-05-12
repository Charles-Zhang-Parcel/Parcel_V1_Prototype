using System;
using System.Collections.Generic;

namespace Parcel.Shared.Framework.ViewModels.BaseNodes
{
    public abstract class PrimitiveNode: ProcessorNode
    {
        #region Public View Properties
        protected string _value;
        public string Value
        {
            get => _value;
            set => SetField(ref _value, value);
        }
        #endregion

        #region Node Interface
        protected readonly OutputConnector ValueOutput = new OutputConnector(typeof(string))
        {
            Title = "Value"
        };
        protected PrimitiveNode()
        {
            ProcessorNodeMemberSerialization = new List<NodeSerializationRoutine>()
            {
                new NodeSerializationRoutine(nameof(Value), () => _value, value => _value = value as string)
            };
            
            Output.Add(ValueOutput);
        }
        #endregion
        
        #region Processor Interface
        public override NodeExecutionResult Execute()
        {
            ProcessorCache[ValueOutput] = new ConnectorCacheDescriptor(_value);
            return new NodeExecutionResult(true, null);
        }
        #endregion
        
        
        #region Serialization
        protected override List<NodeSerializationRoutine> ProcessorNodeMemberSerialization { get; }
        #endregion
    }
}