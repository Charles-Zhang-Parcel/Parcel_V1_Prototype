using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Parcel.Shared.DataTypes;
using Parcel.Shared.Framework;
using Parcel.Shared.Framework.ViewModels;
using Parcel.Shared.Framework.ViewModels.BaseNodes;
using Parcel.Toolbox.DataProcessing.Nodes;
using DataGrid = Parcel.Shared.DataTypes.DataGrid;

namespace Parcel.FrontEnd.NodifyWPF
{
    public partial class PreviewWindow : BaseWindow
    {
        #region Construction
        public PreviewWindow(Window owner, ProcessorNode processorNode)
        {
            Owner = owner;
            Node = processorNode;

            if (processorNode is DataTable)
                DataGridIsReadOnly = false;
            
            InitializeComponent();

            GeneratePreviewForOutput();
        }
        public ProcessorNode Node { get; }
        #endregion

        #region View Properties
        private string _testLabel;
        public string TestLabel
        {
            get => _testLabel;
            set => SetField(ref _testLabel, value);
        }

        private Visibility _infoGridVisibility = Visibility.Visible;
        public Visibility InfoGridVisibility
        {
            get => _infoGridVisibility;
            set => SetField(ref _infoGridVisibility, value);
        }
        private Visibility _dataGridVisibility = Visibility.Visible;
        public Visibility DataGridVisibility
        {
            get => _dataGridVisibility;
            set => SetField(ref _dataGridVisibility, value);
        }

        private string[] _dataGridDataColumns;
        public string[] DataGridDataColumns
        {
            get => _dataGridDataColumns;
            set => SetField(ref _dataGridDataColumns, value);
        }
        private List<dynamic> _dataGridData;
        public List<dynamic> DataGridData
        {
            get => _dataGridData;
            set => SetField(ref _dataGridData, value);
        }
        public bool _dataGridIsReadOnly = true;
        public bool DataGridIsReadOnly
        {
            get => _dataGridIsReadOnly;
            set => SetField(ref _dataGridIsReadOnly, value);   
        }
        #endregion

        #region Interface
        public void Update()
        {
            GeneratePreviewForOutput();
            UpdateLayout();
        }
        #endregion

        #region Events
        private void PreviewWindow_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        #endregion

        #region Routines
        private void GeneratePreviewForOutput()
        {
            WindowGrid.Children.Clear();
            InfoGridVisibility = Visibility.Collapsed;
            DataGridVisibility = Visibility.Collapsed;
            
            OutputConnector output = Node.MainOutput;
            if (Node.ProcessorCache.ContainsKey(output))
            {
                ConnectorCacheDescriptor cache = Node.ProcessorCache[output];
                switch (cache.DataType)
                {
                    case CacheDataType.Generic:
                    case CacheDataType.Boolean:
                    case CacheDataType.Number:
                    case CacheDataType.String:
                        TestLabel = $"{cache.DataObject}";
                        InfoGridVisibility = Visibility.Visible;
                        break;
                    case CacheDataType.ParcelDataGrid:
                        PopulateDataGrid(cache.DataObject as DataGrid);
                        DataGridVisibility = Visibility.Visible;
                        break;
                    default:
                        TestLabel = "No preview is available for this node.";
                        InfoGridVisibility = Visibility.Visible;
                        break;
                }
            }
        }

        private void PopulateDataGrid(DataGrid dataGrid)
        {
            string FormatHeader(string header, string typeName)
            {
                return $"{header} ({typeName})";
            }

            List<dynamic> objects = dataGrid.Rows;
            Dictionary<string, DataGrid.ColumnInfo> columnInfo = dataGrid.GetColumnInfoForDisplay();

            // Collect column names
            IEnumerable<IDictionary<string, object>> rows = objects.OfType<IDictionary<string, object>>();
            DataGridDataColumns = rows.SelectMany(d => d.Keys).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
            // Generate columns
            WpfDataGrid.Columns.Clear();
            foreach (string columnName in DataGridDataColumns)
            {
                // now set up a column and binding for each property
                var column = new DataGridTextColumn 
                {
                    Header = FormatHeader(columnName, columnInfo[columnName].TypeName),
                    Binding = new Binding(columnName)
                };
                WpfDataGrid.Columns.Add(column);
            }

            // Bind object
            DataGridData = objects;
        }

        #endregion

        
    }
}