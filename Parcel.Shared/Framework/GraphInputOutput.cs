using System.Collections.ObjectModel;
using Parcel.Shared.DataTypes;
using Parcel.Shared.Framework.ViewModels;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

namespace Parcel.Shared.Framework
{
    public class GraphInputOutputDefinition: ObservableObject
    {
        #region Properties
        private string _name = "New";
        public string Name
        {
            get => _name;
            set => SetField(ref _name, value);
        }
        private DictionaryEntryType _type = DictionaryEntryType.Number;
        public DictionaryEntryType Type // TODO: This member is not made use of yet; Currently we are just parsing the string according to heuristics
        {
            get => _type;
            set => SetField(ref _type, value);
        }
        private object _defaultValue = 0;
        public object DefaultValue
        {
            get => _defaultValue;
            set => SetField(ref _defaultValue, DataGrid.Preformatting((string)value));  // Currently we are not making use of Type property
        }
        #endregion
    }

    public abstract class GraphInputOutputNodeBase : ProcessorNode
    {
        #region Node Interface
        public GraphInputOutputNodeBase()
        {
            Title = NodeTypeName = "Graph Input Output";

            AddEntry();
            
            AddEntryCommand = new RequeryCommand(
                () => AddEntry(),
                () => true);
            RemoveEntryCommand = new RequeryCommand(
                () => RemoveEntry(),
                () => Definitions.Count > 1);
        }
        #endregion

        #region View Binding/Internal Node Properties
        private ObservableCollection<GraphInputOutputDefinition> _definitions = new ObservableCollection<GraphInputOutputDefinition>();
        public ObservableCollection<GraphInputOutputDefinition> Definitions { get => _definitions; set => SetField(ref _definitions, value); }
        public IProcessorNodeCommand AddEntryCommand { get; }
        public IProcessorNodeCommand RemoveEntryCommand { get; }
        #endregion
        
        #region Routines
        protected abstract void AddEntry();
        protected abstract void RemoveEntry();
        #endregion
        
        #region Processor Interface
        public override OutputConnector MainOutput => Output.Count == 0 ? null : Output[0] as OutputConnector;
        public abstract override NodeExecutionResult Execute();
        #endregion
    }
}