using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Parcel.Shared.DataTypes;
using Parcel.Shared.Framework.ViewModels;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

namespace Parcel.Shared.Framework.Advanced
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
        private CacheDataType _type = CacheDataType.Number;
        public CacheDataType Type
        {
            get => _type;
            set => SetField(ref _type, value);
        }
        #endregion

        #region Payload
        public object Payload { get; set; }
        #endregion

        #region Accessor
        public Type ObjectType => CacheTypeHelper.ConvertToObjectType(Type);
        #endregion
    }

    public abstract class GraphInputOutputNodeBase : ProcessorNode
    {
        #region Node Interface
        protected GraphInputOutputNodeBase()
        {
            Title = NodeTypeName = "Graph Input Output";

            AddEntryCommand = new RequeryCommand(
                AddEntry,
                () => true);
            RemoveEntryCommand = new RequeryCommand(
                RemoveEntry,
                () => Definitions.Count > 1);
            
            ProcessorNodeMemberSerialization = new Dictionary<string, NodeSerializationRoutine>()
            {
                {"Entries", new NodeSerializationRoutine(SerializeEntries,
                    source => DeserializeEntries((List<Tuple<string, int>>)source))}
            };
        }
        #endregion

        #region View Binding/Internal Node Properties
        private ObservableCollection<GraphInputOutputDefinition> _definitions = new ObservableCollection<GraphInputOutputDefinition>();
        public ObservableCollection<GraphInputOutputDefinition> Definitions { get => _definitions; private set => SetField(ref _definitions, value); }
        public IProcessorNodeCommand AddEntryCommand { get; }
        public IProcessorNodeCommand RemoveEntryCommand { get; }
        #endregion
        
        #region Additional View Binding
        protected abstract Action<GraphInputOutputDefinition> DefinitionChanged { get; }
        #endregion
        
        #region Routines
        private void AddEntry()
        {
            string name = $"{NewEntryPrefix} {Definitions.Count + 1}";
            GraphInputOutputDefinition def = new GraphInputOutputDefinition() {Name = name};
            def.PropertyChanged += (sender, args) => DefinitionChanged(sender as GraphInputOutputDefinition);
            
            Definitions.Add(def);
            PostAddEntry(def);
        }
        private void RemoveEntry()
        {
            Definitions.RemoveAt(Definitions.Count - 1);
            PostRemoveEntry();
        }
        protected abstract void PostAddEntry(GraphInputOutputDefinition definition);
        protected abstract void PostRemoveEntry();
        protected abstract string NewEntryPrefix { get; }
        #endregion
        
        #region Processor Interface

        protected abstract override NodeExecutionResult Execute();
        #endregion
        
        #region Serialization
        protected sealed override Dictionary<string, NodeSerializationRoutine> ProcessorNodeMemberSerialization { get; }
        protected abstract void DeserializeFinalize();
        #endregion

        #region Routiens
        private List<Tuple<string, int>> SerializeEntries()
            => Definitions.Select(def => new Tuple<string, int>(def.Name, (int) def.Type))
                .ToList();
        private void DeserializeEntries(List<Tuple<string, int>> source)
        {
            Definitions.AddRange(source.Select(tuple => new GraphInputOutputDefinition()
            {
                Name = tuple.Item1,
                Type = (CacheDataType) tuple.Item2
            }));
            DeserializeFinalize();
        }
        #endregion
    }
}