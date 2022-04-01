using System;
using System.Reflection;
using Parcel.Shared.Framework;

namespace Parcel.Toolbox.DataProcessing
{
    public class DataProcessingHelper: IToolboxEntry
    {
        public string ToolboxName => "Data Processing";
        public string ToolboxAssemblyFullName => Assembly.GetExecutingAssembly().FullName;
        public string[] ExportNames => new string[] { };
    }
}