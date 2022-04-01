namespace Parcel.Shared.Framework
{
    public interface IToolboxEntry
    {
        public string ToolboxName { get; }
        public string ToolboxAssemblyFullName { get; }
        public string[] ExportNames { get; }
    }
}