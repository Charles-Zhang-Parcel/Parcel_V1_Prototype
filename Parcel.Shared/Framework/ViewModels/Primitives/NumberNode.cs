using System.Windows;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

namespace Parcel.Shared.Framework.ViewModels.Primitives
{
    public class NumberNode: PrimitiveNode
    {
        #region View Components
        private double? _number;
        public double? Number
        {
            get => _number;
            set => SetField(ref _number, value);
        }
        #endregion
        
        #region Node Interface
        protected BaseConnector NumberOutput = new OutputConnector(typeof(double))
        {
            Title = "Number"
        }; 
        public NumberNode()
        {
            Title = "Number";
            ValueOutput.IsHidden = true;
            Output.Add(NumberOutput);
        }
        #endregion

        #region Interface
        public override NodeExecutionResult Execute()
        {
            ProcessorCache[NumberOutput] = new ConnectorCacheDescriptor()
            {
                DataObject = _number,
                DataType = CacheDataType.Number 
            };
            
            return base.Execute();
        }
        #endregion
    }
}