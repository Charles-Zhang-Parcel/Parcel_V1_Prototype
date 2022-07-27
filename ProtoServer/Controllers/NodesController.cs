using Microsoft.AspNetCore.Mvc;
using Parcel.Shared;
using Parcel.Shared.Framework;
using Parcel.Shared.Framework.ViewModels.BaseNodes;
using System.Reflection;

namespace ProtoServer.Controllers
{
    [Serializable]
    public class ToolboxNodeDef
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string ToolboxName { get; set; }

        public string[] InputTypes { get; set; }
        public string[] OutputTypes { get; set; }
        public string[] InputNames { get; set; }
        public string[] OutputNames { get; set; }

        public ToolboxNodeDef(string name, string type, string toolboxName, string[] inputTypes, string[] outputTypes, string[] inputNames, string[] outputNames)
        {
            Name = name;
            Type = type;
            ToolboxName = toolboxName;
            InputTypes = inputTypes;
            OutputTypes = outputTypes;
            InputNames = inputNames;
            OutputNames = outputNames;
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class NodesController : ControllerBase
    {
        // GET: api/<Nodes>
        [HttpGet]
        public IEnumerable<ToolboxNodeDef> Get()
        {
            var _registry = new ToolboxRegistry();
            var nodes = new List<ToolboxNodeDef>();

            _registry.RegisterToolbox("Basic", Assembly.GetAssembly(typeof(Parcel.Toolbox.Basic.ToolboxDefinition)));
            _registry.RegisterToolbox("Control Flow", Assembly.GetAssembly(typeof(Parcel.Toolbox.ControlFlow.ToolboxDefinition)));
            _registry.RegisterToolbox("Data Processing", Assembly.GetAssembly(typeof(Parcel.Toolbox.DataProcessing.ToolboxDefinition)));
            _registry.RegisterToolbox("Data Source", Assembly.GetAssembly(typeof(Parcel.Toolbox.DataSource.ToolboxDefinition)));
            _registry.RegisterToolbox("File System", Assembly.GetAssembly(typeof(Parcel.Toolbox.FileSystem.ToolboxDefinition)));
            _registry.RegisterToolbox("Finance", Assembly.GetAssembly(typeof(Parcel.Toolbox.Finance.ToolboxDefinition)));
            _registry.RegisterToolbox("Generator", Assembly.GetAssembly(typeof(Parcel.Toolbox.Generator.ToolboxDefinition)));
            _registry.RegisterToolbox("Graphing", Assembly.GetAssembly(typeof(Parcel.Toolbox.Graphing.ToolboxDefinition)));
            _registry.RegisterToolbox("Logic", Assembly.GetAssembly(typeof(Parcel.Toolbox.Logic.ToolboxDefinition)));
            _registry.RegisterToolbox("Math", Assembly.GetAssembly(typeof(Parcel.Toolbox.Math.ToolboxDefinition)));
            _registry.RegisterToolbox("Plotting", Assembly.GetAssembly(typeof(Parcel.Toolbox.Plotting.ToolboxDefinition)));
            _registry.RegisterToolbox("Present", Assembly.GetAssembly(typeof(Parcel.Toolbox.Present.ToolboxDefinition)));
            _registry.RegisterToolbox("Scripting", Assembly.GetAssembly(typeof(Parcel.Toolbox.Scripting.ToolboxDefinition)));
            _registry.RegisterToolbox("String", Assembly.GetAssembly(typeof(Parcel.Toolbox.String.ToolboxDefinition)));
            _registry.RegisterToolbox("Special", Assembly.GetAssembly(typeof(Parcel.Toolbox.Special.ToolboxDefinition)));

            foreach (string name in _registry.Toolboxes.Keys.OrderBy(k => k))
            {
                string formalName = $"{name.Replace(" ", String.Empty)}";
                string toolboxHelperTypeName = $"Parcel.Toolbox.{formalName}.{formalName}Helper";
                foreach (Type type in _registry.Toolboxes[name]
                    .GetTypes().Where(p => typeof(IToolboxEntry).IsAssignableFrom(p)))
                {
                    IToolboxEntry toolbox = (IToolboxEntry)Activator.CreateInstance(type);
                    if (toolbox == null) continue;

                    foreach (ToolboxNodeExport nodeExport in toolbox.ExportNodes)
                    {
                        if (nodeExport != null)
                        {
                            if (nodeExport.Type.IsAssignableTo(typeof(ProcessorNode)))
                            {
                                ProcessorNode node = (ProcessorNode)Activator.CreateInstance(nodeExport.Type);
                                nodeExport.Toolbox = toolbox;
                                nodes.Add(new ToolboxNodeDef(nodeExport.Name, nodeExport.Type.Name, name,
                                    node.Input.Select(i => i.DataType.Name).ToArray(),
                                    node.Output.Select(i => i.DataType.Name).ToArray(),
                                    node.Input.Select(i => i.Title).ToArray(),
                                    node.Output.Select(i => i.Title).ToArray()));
                            }
                        }
                    }
                    foreach (AutomaticNodeDescriptor definition in toolbox.AutomaticNodes)
                        nodes.Add(definition == null ? null : new ToolboxNodeDef(definition.NodeName, typeof(AutomaticProcessorNode).Name, name, 
                            definition.InputTypes.Select(i => i.ToString()).ToArray(),
                            definition.OutputTypes.Select(i => i.ToString()).ToArray(),
                            definition.InputNames, definition.OutputNames));
                }
            }

            return nodes;
        }
    }
}
