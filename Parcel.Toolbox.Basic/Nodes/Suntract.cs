using Parcel.Shared.DataTypes;
using Parcel.Shared.Framework;
using Parcel.Shared.Framework.ViewModels;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

namespace Parcel.Toolbox.Basic.Nodes
{
    public class Subtract: ProcessorNode
    {
        #region Node Interface
        public readonly BaseConnector Number1Input = new InputConnector(typeof(double))
        {
            Title = "Number 1",
        };
        public readonly BaseConnector Number2Input = new InputConnector(typeof(double))
        {
            Title = "Number 2",
        };
        public readonly BaseConnector ResultOutput = new OutputConnector(typeof(double))
        {
            Title = "Result",
        };
        public Subtract()
        {
            Title = "Subtract";
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
            SubtractParameter parameter = new SubtractParameter()
            {
                InputNumber1 = number1,
                InputNumber2 = number2
            };
            BasicHelper.Subtract(parameter);

            ProcessorCache[ResultOutput] = new ConnectorCacheDescriptor(parameter.OutputNumber);

            Message.Content = $"{number1}-{number2}={parameter.OutputNumber}";
            Message.Type = NodeMessageType.Normal;
            
            return new NodeExecutionResult(true, null);
        }
        #endregion
    }
}