using System.Windows;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

namespace Parcel.Shared.Framework.ViewModels.Primitives
{
    public class NumberNode: PrimitiveNode
    {
        #region View Components
        public double? Number
        {
            get => double.Parse(_value);
            set => SetField(ref _value, value.ToString());
        }
        #endregion
        
        #region Node Interface
        protected BaseConnector NumberOutput = new OutputConnector(typeof(double))
        {
            Title = "Number"
        }; 
        public NumberNode()
        {
            Title = NodeTypeName = "Number";
            ValueOutput.IsHidden = true;
            Output.Add(NumberOutput);
        }
        #endregion

        #region Interface
        public override OutputConnector MainOutput => NumberOutput as OutputConnector;
        public override NodeExecutionResult Execute()
        {
            ProcessorCache[NumberOutput] = new ConnectorCacheDescriptor(Number);
            
            return base.Execute();
        }
        #endregion
    }
}