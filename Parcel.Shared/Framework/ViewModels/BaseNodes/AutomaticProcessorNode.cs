using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using Parcel.Shared.DataTypes;
using Parcel.Shared.Framework.ViewModels.Primitives;

namespace Parcel.Shared.Framework.ViewModels.BaseNodes
{
    /// <summary>
    /// An encapsulation of a base node instance that's generated directly from methods;
    /// We will start with only a single output but there shouldn't be much difficulty outputting more outputs
    /// </summary>
    public class AutomaticProcessorNode: ProcessorNode
    {
        #region Constructor
        public AutomaticProcessorNode()
        {
            ProcessorNodeMemberSerialization = new Dictionary<string, NodeSerializationRoutine>()
            {
                {nameof(AutomaticNodeType), new NodeSerializationRoutine(() => AutomaticNodeType, value => AutomaticNodeType = value as string)},
                {nameof(ToolboxFullName), new NodeSerializationRoutine(() => ToolboxFullName, value => ToolboxFullName = value as string)},
                {nameof(InputTypes), new NodeSerializationRoutine(() => InputTypes, value => InputTypes = value as CacheDataType[])},
                {nameof(OutputTypes), new NodeSerializationRoutine(() => OutputTypes, value => OutputTypes = value as CacheDataType[])},
            };
        }
        public AutomaticProcessorNode(AutomaticNodeDescriptor descriptor, IToolboxEntry toolbox)
            :this()
        {
            // Serialization
            AutomaticNodeType = descriptor.NodeName;
            ToolboxFullName = toolbox.GetType().AssemblyQualifiedName;
            InputTypes = descriptor.InputTypes;
            OutputTypes = descriptor.OutputTypes;
            
            // Population
            PopulateInputsOutputs();
        }
        #endregion

        #region Routines
        private Func<object[], object[]> RetrieveCallMarshal()
        {
            try
            {
                IToolboxEntry toolbox = (IToolboxEntry) Activator.CreateInstance(Type.GetType(ToolboxFullName));
                AutomaticNodeDescriptor descriptor = toolbox.AutomaticNodes.Single(an => an != null && an.NodeName == AutomaticNodeType);
                return descriptor.CallMarshal;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Failed to retrieve node.");
            }
        }
        private void PopulateInputsOutputs()
        {
            Title = NodeTypeName = AutomaticNodeType;
            foreach (CacheDataType inputType in InputTypes)
            {
                switch (inputType)
                {
                    case CacheDataType.Boolean:
                        Input.Add(new PrimitiveBooleanInputConnector() {Title = "Bool"});
                        break;
                    case CacheDataType.Number:
                        Input.Add(new PrimitiveNumberInputConnector() {Title = "Number"});
                        break;
                    case CacheDataType.String:
                    case CacheDataType.DateTime:
                    case CacheDataType.ParcelDataGrid:
                    case CacheDataType.Array:
                    case CacheDataType.Generic:
                    case CacheDataType.BatchJob:
                    case CacheDataType.ServerConfig:
                        throw new NotImplementedException();
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            foreach (CacheDataType outputType in OutputTypes)
            {
                switch (outputType)
                {
                    case CacheDataType.Boolean:
                        Output.Add(new OutputConnector(typeof(bool)) {Title = "Truth"});
                        break;
                    case CacheDataType.Number:
                        Output.Add(new OutputConnector(typeof(double)) {Title = "Number"});
                        break;
                    case CacheDataType.String:
                    case CacheDataType.DateTime:
                    case CacheDataType.ParcelDataGrid:
                    case CacheDataType.Array:
                    case CacheDataType.Generic:
                    case CacheDataType.BatchJob:
                    case CacheDataType.ServerConfig:
                        throw new NotImplementedException();
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        #endregion

        #region Properties
        private string AutomaticNodeType { get; set; }
        private string ToolboxFullName { get; set; }
        private CacheDataType[] InputTypes { get; set; }
        private CacheDataType[] OutputTypes { get; set; }
        #endregion

        #region Processor Interface
        protected override NodeExecutionResult Execute()
        {
            Dictionary<OutputConnector, object> cache = new Dictionary<OutputConnector, object>();
            
            Func<object[], object[]> marshal = RetrieveCallMarshal();
            object[] outputs = marshal.Invoke(Input.Select(i => i.FetchInputValue<object>()).ToArray());
            for (int index = 0; index < outputs.Length; index++)
            {
                object output = outputs[index];
                OutputConnector connector = Output[index];
                cache[connector] = output;
            }

            return new NodeExecutionResult(new NodeMessage(), cache);
        }
        #endregion

        #region Serialization
        protected sealed override Dictionary<string, NodeSerializationRoutine> ProcessorNodeMemberSerialization { get; }
        internal override void PostDeserialization()
        {
            base.PostDeserialization();
            PopulateInputsOutputs();
        }
        #endregion
        
        #region Auto-Connect Interface
        public override Tuple<ToolboxNodeExport, Vector, InputConnector>[] AutoGenerateNodes
        {
            get
            {
                List<Tuple<ToolboxNodeExport, Vector, InputConnector>> auto =
                    new List<Tuple<ToolboxNodeExport, Vector, InputConnector>>();
                for (int i = 0; i < Input.Count; i++)
                {
                    if(Input[i].Connections.Count != 0) continue;

                    Type nodeType;
                    switch (InputTypes[i])
                    {
                        case CacheDataType.Boolean:
                            nodeType = typeof(BooleanNode);
                            break;
                        case CacheDataType.Number:
                            nodeType = typeof(NumberNode);
                            break;
                        case CacheDataType.String:
                            nodeType = typeof(StringNode);
                            break;
                        case CacheDataType.DateTime:
                            nodeType = typeof(DateTimeNode);
                            break;
                        case CacheDataType.ParcelDataGrid:
                        case CacheDataType.Array:
                        case CacheDataType.Generic:
                        case CacheDataType.BatchJob:
                        case CacheDataType.ServerConfig:
                            throw new NotImplementedException();
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    ToolboxNodeExport toolDef = new ToolboxNodeExport(Input[i].Title, nodeType);
                    auto.Add(new Tuple<ToolboxNodeExport, Vector, InputConnector>(toolDef, new Vector(-100, -50 + (i - 1) * 50), Input[i]));
                }
                return auto.ToArray();
            }
        }
        public override bool ShouldHaveConnection => Input.Count > 0 && Input.Any(i => i.Connections.Count == 0);
        #endregion
    }
}