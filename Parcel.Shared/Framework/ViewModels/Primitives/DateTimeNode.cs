using System;
using System.Windows;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

namespace Parcel.Shared.Framework.ViewModels.Primitives
{
    public class DateTimeNode: PrimitiveNode
    {
        #region View Components
        public DateTime DateTime
        {
            get
            {
                if (DateTime.TryParse(_value, out DateTime result))
                    return result;
                return DateTime.Now;
            }
            set => SetField(ref _value, value.ToString());
        }
        #endregion
        
        #region Node Interface
        protected BaseConnector DateTimeOutput = new OutputConnector(typeof(DateTime))
        {
            Title = "DateTime"
        }; 
        public DateTimeNode()
        {
            Title = NodeTypeName = "DateTime";
            DateTime = DateTime.Now;
            ValueOutput.IsHidden = true;
            Output.Add(DateTimeOutput);
        }
        #endregion

        #region Interface
        public override OutputConnector MainOutput => DateTimeOutput as OutputConnector;
        public override NodeExecutionResult Execute()
        {
            ProcessorCache[DateTimeOutput] = new ConnectorCacheDescriptor(DateTime);
            
            return base.Execute();
        }
        #endregion
    }
}