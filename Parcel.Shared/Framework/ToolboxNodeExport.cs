using System;

namespace Parcel.Shared.Framework
{
    public class ToolboxNodeExport
    {
        public string Name { get; set; }
        public Type Type { get; set; }

        public ToolboxNodeExport(string name, Type type)
        {
            Name = name;
            Type = type;
        }
    }
}