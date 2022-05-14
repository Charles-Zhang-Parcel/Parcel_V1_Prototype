using System.Collections.Generic;
using Parcel.Shared.Framework;
using Parcel.Shared.Framework.ViewModels;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

namespace Parcel.Toolbox.Math.Nodes
{
    public class Multiply: ProcessorNode
    {
        #region Node Interface
        private readonly InputConnector _number1Input = new InputConnector(typeof(double))
        {
            Title = "Number 1",
        };
        private readonly InputConnector _number2Input = new InputConnector(typeof(double))
        {
            Title = "Number 2",
        };
        private readonly OutputConnector _resultOutput = new OutputConnector(typeof(double))
        {
            Title = "Result",
        };
        public Multiply()
        {
            Title = NodeTypeName = "Multiply";
            Input.Add(_number1Input);
            Input.Add(_number2Input);
            Output.Add(_resultOutput);
        }
        #endregion
        
        #region Processor Interface
        public override OutputConnector MainOutput => _resultOutput as OutputConnector;

        protected override NodeExecutionResult Execute()
        {
            double number1 = _number1Input.FetchInputValue<double>();
            double number2 = _number2Input.FetchInputValue<double>();
            MultiplyParameter parameter = new MultiplyParameter()
            {
                InputNumber1 = number1,
                InputNumber2 = number2
            };
            MathHelper.Multiply(parameter);

            return new NodeExecutionResult(new NodeMessage($"{number1}×{number2}={parameter.OutputNumber}"), new Dictionary<OutputConnector, object>()
            {
                {_resultOutput, parameter.OutputNumber}
            });
        }
        #endregion
        
        #region Serialization
        protected override Dictionary<string, NodeSerializationRoutine> ProcessorNodeMemberSerialization { get; } =
            null;
        protected override NodeSerializationRoutine InputConnectorsSerialization { get; } = null;
        #endregion
    }
}