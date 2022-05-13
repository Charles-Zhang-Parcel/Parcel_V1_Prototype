using System.Collections.Generic;
using Parcel.Shared.Framework;
using Parcel.Shared.Framework.ViewModels;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

namespace Parcel.Toolbox.Math.Nodes
{
    public class Calculator: ProcessorNode
    {
        #region Node Interface
        private readonly OutputConnector _resultOutput = new OutputConnector(typeof(double))
        {
            Title = "Result",
        };
        public Calculator()
        {
            Title = NodeTypeName = "Calculator";
            Output.Add(_resultOutput);
        }
        #endregion
        
        #region Processor Interface
        protected override NodeExecutionResult Execute()
        {
            double result = 0;

            return new NodeExecutionResult(new NodeMessage($"{result}", NodeMessageType.Normal), new Dictionary<OutputConnector, object>()
            {
                {_resultOutput, result}
            });
        }
        #endregion
    }
}