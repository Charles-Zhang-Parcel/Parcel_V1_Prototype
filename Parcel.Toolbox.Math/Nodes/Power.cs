using Parcel.Shared.Framework;
using Parcel.Shared.Framework.ViewModels;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

namespace Parcel.Toolbox.Math.Nodes
{
    public class Power: ProcessorNode
    {
        #region Node Interface
        public readonly InputConnector Number1Input = new InputConnector(typeof(double))
        {
            Title = "Number 1",
        };
        public readonly InputConnector Number2Input = new InputConnector(typeof(double))
        {
            Title = "Number 2",
        };
        public readonly OutputConnector ResultOutput = new OutputConnector(typeof(double))
        {
            Title = "Result",
        };
        public Power()
        {
            Title = NodeTypeName = "Power";
            Input.Add(Number1Input);
            Input.Add(Number2Input);
            Output.Add(ResultOutput);
        }
        #endregion
        
        #region Processor Interface
        public override OutputConnector MainOutput => ResultOutput as OutputConnector;
        public override NodeExecutionResult Execute()
        {
            double number1 = Number1Input.FetchInputValue<double>();
            double number2 = Number2Input.FetchInputValue<double>();
            PowerParameter parameter = new PowerParameter()
            {
                InputNumber1 = number1,
                InputNumber2 = number2
            };
            MathHelper.Power(parameter);

            ProcessorCache[ResultOutput] = new ConnectorCacheDescriptor(parameter.OutputNumber);

            Message.Content = $"{number1}^{number2}={parameter.OutputNumber}";
            Message.Type = NodeMessageType.Normal;
            
            return new NodeExecutionResult(true, null);
        }
        #endregion
    }
}