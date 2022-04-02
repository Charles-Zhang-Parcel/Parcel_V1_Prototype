using Parcel.Shared.DataTypes;
using Parcel.Shared.Framework;
using Parcel.Shared.Framework.ViewModels;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

namespace Parcel.Toolbox.DataProcessing.Nodes
{
    public class CSV: ProcessorNode
    {
        #region Node Interface
        protected BaseConnector PathInput = new BaseConnector()
        {
            Title = "Path"
        };
        protected BaseConnector DataTableOutput = new BaseConnector()
        {
            Title = "Data Table"
        }; 
        public CSV()
        {
            Title = "CSV";
            Input.Add(PathInput);
            Output.Add(DataTableOutput);
        }
        #endregion
        
        #region Processor Interface
        public override NodeExecutionResult Execute()
        {
            ProcessorCache[DataTableOutput] = new ConnectorCacheDescriptor()
            {
                DataObject = new DataGrid(),
                DataType = CacheDataType.ParcelDataGrid 
            };
            return new NodeExecutionResult(true, null);
        }
        #endregion
    }
}