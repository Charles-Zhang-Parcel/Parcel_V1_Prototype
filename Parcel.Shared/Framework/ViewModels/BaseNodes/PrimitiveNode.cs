using System;

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
        protected BaseConnector ValueOutput = new OutputConnector(typeof(string))
        {
            Title = "Value"
        }; 
        public PrimitiveNode()
        {
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
    }
}