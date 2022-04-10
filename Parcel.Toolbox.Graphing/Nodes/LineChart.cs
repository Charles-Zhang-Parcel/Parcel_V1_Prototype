using System.Collections.Generic;
using System.Linq;
using Parcel.Shared;
using Parcel.Shared.DataTypes;
using Parcel.Shared.Framework;
using Parcel.Shared.Framework.ViewModels;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

namespace Parcel.Toolbox.Graphing.Nodes
{
    public class LineChart: ProcessorNode, IWebPreviewProcessorNode, INodeProperty
    {
        #region Node Interface
        public readonly InputConnector DataTableInput = new InputConnector(typeof(DataGrid))
        {
            Title = "Data Table"
        };
        public readonly OutputConnector ServerConfigOutput = new OutputConnector(typeof(ServerConfig))
        {
            Title = "Present"
        };
        public LineChart()
        {
            _editors = new List<PropertyEditor>()
            {
                new PropertyEditor("Code", PropertyEditorType.TextBox, () => _chartTitle, v => _chartTitle = (string)v)
            };
            
            Title = NodeTypeName = ChartTitle = "LineChart";
            Input.Add(DataTableInput);
            Output.Add(ServerConfigOutput);
        }
        #endregion

        #region View Binding/Internal Node Properties
        public string _chartTitle;
        public string ChartTitle
        {
            get => _chartTitle;
            set => SetField(ref _chartTitle, value);
        }
        #endregion

        #region Property Editor Interface

        private readonly List<PropertyEditor> _editors;
        public List<PropertyEditor> Editors => _editors;
        #endregion
        
        #region Processor Interface
        public override OutputConnector MainOutput => ServerConfigOutput;
        public override NodeExecutionResult Execute()
        {
            ServerConfig config = new ServerConfig()
            {
                ChartType = ChartType.Line,
                ContentType = CacheDataType.ParcelDataGrid,
                DataGridContent = DataTableInput.FetchInputValue<DataGrid>(),
            };
            WebHostRuntime.Singleton.CurrentLayout = config;

            Message.Content = $"Ready.";
            Message.Type = NodeMessageType.Normal;
            
            ((IWebPreviewProcessorNode)this).OpenPreview("Present");
            return new NodeExecutionResult(true, null);
        }
        #endregion
    }
}