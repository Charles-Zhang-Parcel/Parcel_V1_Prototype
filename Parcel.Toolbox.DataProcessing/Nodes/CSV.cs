using Parcel.Shared.DataTypes;
using Parcel.Shared.Framework;
using Parcel.Shared.Framework.ViewModels;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

namespace Parcel.Toolbox.DataProcessing.Nodes
{
    public class CSV: ProcessorNode
    {
        #region Node Interface
        public readonly BaseConnector PathInput = new InputConnector(typeof(string))
        {
            Title = "Path",
        };
        public readonly  BaseConnector HeaderInput = new InputConnector(typeof(bool))
        {
            Title = "Contains Header"
        };
        public readonly BaseConnector DataTableOutput = new OutputConnector(typeof(DataGrid))
        {
            Title = "Data Table"
        }; 
        public CSV()
        {
            Title = "CSV";
            Input.Add(PathInput);
            Input.Add(HeaderInput);
            Output.Add(DataTableOutput);

            Message.Content = "Test"; // TODO: Not working
        }
        #endregion
        
        #region Processor Interface
        public override NodeExecutionResult Execute()
        {
            CSVParameter parameter = new CSVParameter()
            {
                InputPath = PathInput.FetchInputValue<string>(),
                InputContainsHeader = HeaderInput.FetchInputValue<bool>()
            };
            DataProcessingHelper.CSV(parameter);
            
            ProcessorCache[DataTableOutput] = new ConnectorCacheDescriptor()
            {
                DataObject = new DataGrid(),
                DataType = CacheDataType.ParcelDataGrid 
            };

            Message.Content = "Loaded.";
            Message.Type = NodeMessageType.Normal;
            
            return new NodeExecutionResult(true, null);
        }
        #endregion
    }
}