using System.Windows;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

namespace Parcel.Shared.Framework.ViewModels.Primitives
{
    public class BooleanNode: PrimitiveNode
    {
        #region View Components
        public bool Boolean
        {
            get => bool.Parse(_value);
            set => SetField(ref _value, value.ToString());
        }
        #endregion
        
        #region Node Interface
        protected BaseConnector TruthOutput = new OutputConnector(typeof(bool))
        {
            Title = "Truth"
        }; 
        public BooleanNode()
        {
            Title = "Boolean";
            ValueOutput.IsHidden = true;
            Output.Add(TruthOutput);
        }
        #endregion

        #region Interface
        public override OutputConnector MainOutput => TruthOutput as OutputConnector;
        public override NodeExecutionResult Execute()
        {
            ProcessorCache[TruthOutput] = new ConnectorCacheDescriptor()
            {
                DataObject = Boolean,
                DataType = CacheDataType.Boolean 
            };
            
            return base.Execute();
        }
        #endregion
    }
}