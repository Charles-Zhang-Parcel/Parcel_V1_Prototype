using Parcel.Shared.DataTypes;
using Parcel.Shared.Framework;
using Parcel.Shared.Framework.ViewModels;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

namespace Parcel.Toolbox.Basic.Nodes
{
    public class Preview: ProcessorNode
    {
        #region Node Interface
        public readonly BaseConnector ObjectInput = new InputConnector(typeof(object))
        {
            Title = "Object",
        };
        public readonly BaseConnector ObjectOutput = new OutputConnector(typeof(object))
        {
            Title = "Object",
        };
        public Preview()
        {
            Title = "Preview";
            Input.Add(ObjectInput);
            Output.Add(ObjectOutput);
        }
        #endregion
        
        #region Processor Interface
        public override OutputConnector MainOutput => ObjectOutput as OutputConnector;
        public override NodeExecutionResult Execute()
        {
            object obj = ObjectInput.FetchInputValue<object>();
            ProcessorCache[ObjectOutput] = new ConnectorCacheDescriptor()
            {
                DataObject = obj,
                DataType = CacheDataType.Generic 
            };

            Message.Content = obj.ToString();
            Message.Type = NodeMessageType.Normal;
            
            return new NodeExecutionResult(true, null);
        }
        #endregion
    }
}