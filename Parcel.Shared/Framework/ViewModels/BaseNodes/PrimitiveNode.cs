using System;

namespace Parcel.Shared.Framework.ViewModels.BaseNodes
{
    public class PrimitiveNode: ProcessorNode
    {
        #region Public View Properties
        private string _value;
        public string Value
        {
            get => _value;
            set => SetField(ref _value, value);
        }
        #endregion

        #region Node Interface
        protected BaseConnector ValueOutput = new OutputConnector(typeof(string))
        {
            Title = "Value",
            Shape = ConnectorShape.Circle
        }; 
        public PrimitiveNode()
        {
            Output.Add(ValueOutput);
        }
        #endregion
        
        #region Processor Interface
        public override NodeExecutionResult Execute()
        {
            ProcessorCache[ValueOutput] = new ConnectorCacheDescriptor()
            {
                DataObject = _value,
                DataType = CacheDataType.String 
            };
            return new NodeExecutionResult(true, null);
        }
        #endregion
    }
}