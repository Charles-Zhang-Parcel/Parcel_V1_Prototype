using System.Reflection;
using Parcel.Shared.Framework;

namespace Parcel.Toolbox.Basic.AutomaticNodes
{
    public class Test: IToolboxEntry
    {
        #region Interface
        public string ToolboxName => "Test";
        public string ToolboxAssemblyFullName => Assembly.GetExecutingAssembly().FullName;
        public ToolboxNodeExport[] ExportNodes => new ToolboxNodeExport[]{};
        public AutomaticNodeDescriptor[] AutomaticNodes => new AutomaticNodeDescriptor[]
        {
            new AutomaticNodeDescriptor("Add", new []{typeof (double), typeof(double)}, typeof(double), objects => Add((double)objects[0], (double)objects[1]))
        };
        #endregion
        
        public static double Add(double input1, double input2)
        {
            return input1 + input2;
        }
    }
}