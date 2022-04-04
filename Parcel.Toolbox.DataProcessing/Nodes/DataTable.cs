using Parcel.Shared.DataTypes;
using Parcel.Shared.Framework;
using Parcel.Shared.Framework.ViewModels;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

namespace Parcel.Toolbox.DataProcessing.Nodes
{
    public class DataTable: ProcessorNode
    {
        #region Node Interface
        public readonly BaseConnector PathInput = new InputConnector(typeof(string))
        {
            Title = "Path",
        };
        public readonly BaseConnector DataTableOutput = new OutputConnector(typeof(DataGrid))
        {
            Title = "Data Table"
        }; 
        public DataTable()
        {
            Title = "Data Table";
            Input.Add(PathInput);
            Output.Add(DataTableOutput);
        }
        #endregion
        
        #region Processor Interface
        public override OutputConnector MainOutput => DataTableOutput as OutputConnector;
        public override NodeExecutionResult Execute()
        {
            // Read from file
            // ...
            
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