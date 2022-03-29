using System.Collections.Generic;
using Parcel.ApplicationState;
using Parcel.SFMLApplication.BaseClasses;
using Parcel.SFMLApplication.Controls;
using Parcel.SFMLApplication.Windows;
using Parcel.Shared;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Parcel.SFMLApplication
{
    public class SFMLMainApplication
    {
        #region Interface
        public SFMLMainApplication(ApplicationRuntimeContext appState)
        {
            ApplicationRuntimeContext = appState;
            
            if (ApplicationRuntimeContext.SFMLRenderingContext == null)
                ApplicationRuntimeContext.InitializeRenderingContext();
            ApplicationRuntimeContext.MainGUIApplication = this;
        }
        public void Run()
        {
            new TestWindow(new VideoMode(1024, 768), WindowTitle, Styles.None).Run();
        }
        #endregion

        #region Configurations
        const string WindowTitle = "Somewhere 2 - Simpler & Better";
        #endregion

        #region Members
        private ApplicationRuntimeContext ApplicationRuntimeContext { get; }
        private SFMLRenderingContext RenderingContext => ApplicationRuntimeContext.SFMLRenderingContext;
        #endregion
    }
}