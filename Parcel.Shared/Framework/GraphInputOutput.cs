using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
            set => SetField(ref _defaultValue, value is string s ? DataGrid.Preformatting(s) : value);  // Currently we are not making use of Type property
        }
        #endregion
    }

    public abstract class GraphInputOutputNodeBase : ProcessorNode
    {
        #region Node Interface
        public GraphInputOutputNodeBase()
        {
            Title = NodeTypeName = "Graph Input Output";

            AddEntryCommand = new RequeryCommand(
                () => AddEntry(),
                () => true);
            RemoveEntryCommand = new RequeryCommand(
                () => RemoveEntry(),
                () => Definitions.Count > 1);
            
            ProcessorNodeMemberSerialization = new Dictionary<string, NodeSerializationRoutine>()
            {
                {"Entries", new NodeSerializationRoutine(() => SerializeEntries(),
                    source => DeserializeEntries((List<Tuple<string, int, object>>)source))}
            };
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
        
        #region Serialization
        protected sealed override Dictionary<string, NodeSerializationRoutine> ProcessorNodeMemberSerialization { get; }
        private List<Tuple<string, int, object>> SerializeEntries()
            => Definitions.Select(def => new Tuple<string, int, object>(def.Name, (int) def.Type, def.DefaultValue))
                .ToList();
        private void DeserializeEntries(List<Tuple<string, int, object>> source)
        {
            Definitions.AddRange(source.Select(tuple => new GraphInputOutputDefinition()
            {
                Name = tuple.Item1,
                Type = (DictionaryEntryType) tuple.Item2,
                DefaultValue = tuple.Item3
            }));
            DeserializeFinalize();
        }
        protected abstract void DeserializeFinalize();
        #endregion
    }
}