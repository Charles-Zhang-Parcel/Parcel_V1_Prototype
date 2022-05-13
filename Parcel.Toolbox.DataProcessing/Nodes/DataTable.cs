using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Parcel.Shared.DataTypes;
using Parcel.Shared.Framework;
using Parcel.Shared.Framework.ViewModels;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

namespace Parcel.Toolbox.DataProcessing.Nodes
{
    /// <summary>
    /// TODO: Editing for Data Table node is not working yet at this moment due to a transform as Expando object during presentation
    /// </summary>
    public class DataTableFieldDefinition: ObservableObject
    {
        #region Properties
        private string _name = "New Field";
        public string Name
        {
            get => _name;
            set => SetField(ref _name, value);
        }
        private DictionaryEntryType _type = DictionaryEntryType.Number;
        public DictionaryEntryType Type
        {
            get => _type;
            set => SetField(ref _type, value);
        }
        #endregion
    }
    
    public class DataTable: ProcessorNode
    {
        #region Node Interface
        private readonly OutputConnector _dataTableOutput = new OutputConnector(typeof(DataGrid))
        {
            Title = "Data Table"
        }; 
        public DataTable()
        {
            Definitions = new ObservableCollection<DataTableFieldDefinition>()
            {
                new DataTableFieldDefinition() {Name = "New Field"}
            };
            
            AddEntryCommand = new RequeryCommand(
                () => Definitions.Add(new DataTableFieldDefinition() {Name = $"New Field {Definitions.Count + 1}"} ),
                () => true);
            RemoveEntryCommand = new RequeryCommand(
                () => Definitions.RemoveAt(Definitions.Count - 1),
                () => Definitions.Count > 1);
            
            Title = NodeTypeName = "Data Table";
            Output.Add(_dataTableOutput);
        }
        #endregion
        
        #region View Binding/Internal Node Properties
        private DataGrid _dataGrid = new DataGrid();
        public DataGrid DataGrid
        {
            get => _dataGrid;
            set => SetField(ref _dataGrid, value);
        }
        private ObservableCollection<DataTableFieldDefinition> _definitions;
        public ObservableCollection<DataTableFieldDefinition> Definitions { get => _definitions; set => SetField(ref _definitions, value); }
        public IProcessorNodeCommand AddEntryCommand { get; }
        public IProcessorNodeCommand RemoveEntryCommand { get; }
        #endregion

        #region Processor Interface
        public override OutputConnector MainOutput => _dataTableOutput as OutputConnector;

        protected override NodeExecutionResult Execute()
        {
            // Update columns
            foreach (DataTableFieldDefinition definition in Definitions)
            {
                if (_dataGrid.Columns.All(c => c.Header != definition.Name))
                {
                    var column = _dataGrid.AddColumn(definition.Name);
                    switch (definition.Type)
                    {
                        case DictionaryEntryType.Number:
                            column.Add((double)0.0);
                            break;
                        case DictionaryEntryType.String:
                            column.Add(string.Empty);
                            break;
                        case DictionaryEntryType.Boolean:
                            column.Add(false);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            foreach (string extra in _dataGrid.Columns
                .Select(c=>c.Header)
                .Except(Definitions.Select(d => d.Name)).ToArray())
                _dataGrid.RemoveColumn(extra);

            return new NodeExecutionResult(new NodeMessage($"{_dataGrid.ColumnCount} Fields."), new Dictionary<OutputConnector, object>()
            {
                {_dataTableOutput, _dataGrid}
            });
        }
        #endregion
    }
}