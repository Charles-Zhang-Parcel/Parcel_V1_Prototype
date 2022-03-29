using System;
using Parcel.SFMLApplication.BaseClasses;
using Parcel.SFMLApplication.Controls;
using SFML.Window;

namespace Parcel.SFMLApplication.Windows
{
    public class TestWindow: SFMLRenderWindow
    {
        #region Constructors
        public TestWindow(VideoMode mode, string title) : base(mode, title)
        {
        }

        public TestWindow(VideoMode mode, string title, Styles style) : base(mode, title, style)
        {
        }

        public TestWindow(VideoMode mode, string title, Styles style, ContextSettings settings) : base(mode, title, style, settings)
        {
        }

        public TestWindow(IntPtr handle) : base(handle)
        {
        }

        public TestWindow(IntPtr handle, ContextSettings settings) : base(handle, settings)
        {
        }
        #endregion


        protected override void CreateControls()
        {
            Controls.Add(new Button(this, "Test TestJ"));
            // Controls.Add(new Node(this, "Read CSV"));
        }
    }
}