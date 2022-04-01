using System;
using System.Reflection;
using Parcel.Shared.Framework;

namespace Parcel.Toolbox.Basic
{
    public class BasicHelper: IToolboxEntry
    {
        public string ToolboxName => "Basic";
        public string ToolboxAssemblyFullName => Assembly.GetExecutingAssembly().FullName;
        public string[] ExportNames => new string[] { "Group", "Comment" };
    }
}