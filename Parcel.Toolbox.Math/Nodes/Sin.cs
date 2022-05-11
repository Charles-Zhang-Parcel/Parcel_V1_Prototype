using Parcel.Shared.Framework;
using Parcel.Shared.Framework.ViewModels;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

namespace Parcel.Toolbox.Math.Nodes
{
    public class Sin: ProcessorNode
    {
        #region Node Interface
        public readonly BaseConnector AngleInput = new InputConnector(typeof(double))
        {
            Title = "Angle",
        };
        public readonly BaseConnector ResultOutput = new OutputConnector(typeof(double))
        {
            Title = "Result",
        };
        public Sin()
        {
            Title = NodeTypeName = "Sin";
            Input.Add(AngleInput);
            Output.Add(ResultOutput);
        }
        #endregion
        
        #region Processor Interface
        public override OutputConnector MainOutput => ResultOutput as OutputConnector;
        public override NodeExecutionResult Execute()
        {
            double angle = AngleInput.FetchInputValue<double>();
            SinParameter parameter = new SinParameter()
            {
                InputAngle = angle,
            };
            MathHelper.Sin(parameter);

            ProcessorCache[ResultOutput] = new ConnectorCacheDescriptor(parameter.OutputNumber);

            Message.Content = $"sin({angle})={parameter.OutputNumber}";
            Message.Type = NodeMessageType.Normal;
            
            return new NodeExecutionResult(true, null);
        }
        #endregion
    }
}