using System.Windows;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

namespace Parcel.Shared.Framework.ViewModels.Primitives
{
    public class BooleanNode: PrimitiveNode
    {
        #region View Components
        private bool? _boolean;
        public bool? Boolean
        {
            get => _boolean;
            set => SetField(ref _boolean, value);
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
        public override NodeExecutionResult Execute()
        {
            ProcessorCache[TruthOutput] = new ConnectorCacheDescriptor()
            {
                DataObject = _boolean,
                DataType = CacheDataType.Boolean 
            };
            
            return base.Execute();
        }
        #endregion
    }
}