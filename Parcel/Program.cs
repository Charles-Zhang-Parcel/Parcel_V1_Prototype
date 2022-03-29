using System;
using System.Windows.Threading;
using Parcel.ApplicationState;
using Parcel.SFMLApplication;
using Parcel.Shared;

namespace Parcel
{
    internal static class Program
    {
        [STAThreadAttribute]
        private static void Main(string[] args)
        {
            // Initialize application data
            ApplicationRuntimeContext runtimeContext = new ApplicationRuntimeContext()
            {
                STADispatcher = Dispatcher.CurrentDispatcher
            };
            
            new SFMLMainApplication(runtimeContext).Run();
        }
        private static void CreateHybridHost(ApplicationRuntimeContext runtimeContext)
        {
            
        }
    }
}