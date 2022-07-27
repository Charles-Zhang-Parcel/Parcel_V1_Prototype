using Parcel.Shared.Framework.ViewModels.BaseNodes;

namespace Parcel.Shared.Framework.ViewModels.Primitives
{
    public class StringNode: PrimitiveNode
    {
        public StringNode()
        {
            Title = NodeTypeName = "String";
        }

        public override OutputConnector MainOutput => ValueOutput as OutputConnector;
    }
}