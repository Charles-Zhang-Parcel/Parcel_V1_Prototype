using Parcel.Shared.Framework.ViewModels;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

namespace Parcel.Toolbox.DataProcessing.Nodes
{
    public class CSV: ProcessorNode
    {
        public CSV()
        {
            Title = "CSV";
            Input.Add(new BaseConnector()
            {
                Title = "Path"
            });
            Output.Add(new BaseConnector()
            {
                Title = "Data Table"
            });
        }
    }
}