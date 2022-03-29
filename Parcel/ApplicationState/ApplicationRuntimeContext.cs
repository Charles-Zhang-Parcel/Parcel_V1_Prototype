using System;
using System.Windows.Threading;
using Parcel.SFMLApplication;
using Parcel.Shared;

namespace Parcel.ApplicationState
{
    public class ApplicationRuntimeContext
    {
        #region Constructor
        public ApplicationRuntimeContext()
        {
            if (Singleton == null)
                Singleton = this;
            else
            {
                throw new InvalidOperationException("RuntimeData is already initialized! Singleton is not null.");
            }

            RuntimeData = new ParcelRuntime();
        }
        #endregion
        
        #region Global Contexts
        public ParcelRuntime RuntimeData { get; set; } 
        public SFMLRenderingContext SFMLRenderingContext { get; set; }
        public SFMLMainApplication MainGUIApplication { get; set; }
        public Dispatcher STADispatcher { get; set; }
        public static ApplicationRuntimeContext Singleton { get; set; }
        #endregion

        #region Interface
        public void InitializeRenderingContext()
        {
            SFMLRenderingContext = new SFMLRenderingContext()
            {
                MainWindow = null,
                BasicRendering = BasicRenderingInfrastructure.Setup()
            };
        }
        #endregion
    }
}