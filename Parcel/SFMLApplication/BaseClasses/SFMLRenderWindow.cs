using System;
using System.Collections.Generic;
using Parcel.ApplicationState;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Parcel.SFMLApplication.BaseClasses
{
    public abstract class SFMLRenderWindow: RenderWindow
    {
        #region Constructors

        protected SFMLRenderWindow(VideoMode mode, string title) : base(mode, title)
            => InitializeWindow();

        protected SFMLRenderWindow(VideoMode mode, string title, Styles style) : base(mode, title, style)
            => InitializeWindow();

        protected SFMLRenderWindow(VideoMode mode, string title, Styles style, ContextSettings settings) : base(mode, title, style, settings)
            => InitializeWindow();

        protected SFMLRenderWindow(IntPtr handle) : base(handle)
            => InitializeWindow();

        protected SFMLRenderWindow(IntPtr handle, ContextSettings settings) : base(handle, settings)
            => InitializeWindow();

        private void InitializeWindow()
        {
            InitializeWindowHandlers();
            CreateControls();
            InitializeDrawContexts();
            PlayIntro();
        }
        #endregion
        
        #region Members
        protected List<SFMLControl> Controls { get; set; } = new List<SFMLControl>();
        #endregion

        #region States
        private Vector2i MoveWindowAnchor { get; set; }
        private bool MoveWindow { get; set; }
        #endregion

        #region Event Listeners
        private List<SFMLControl> MouseActionListeners { get; set; } = new List<SFMLControl>();

        public void RegisterMouseEventListener(SFMLControl control)
        {
            if(control.Owner == this)
                MouseActionListeners.Add(control);
        }
        #endregion

        #region Interface Calls
        public void Run()
        {
            while (this.IsOpen)
            {
                this.Clear();
                this.DrawContents();
                this.Display();
                this.WaitAndDispatchEvents();
            }
        }
        public void DrawContents()
        {
            // Draw current screen
            foreach (SFMLControl control in Controls)
            {
                control.Draw(this);
            }
        }
        #endregion

        #region Routines
        protected virtual void InitializeWindowHandlers()
        {
            this.Closed += (sender, eventArgs) => this.Close();
            this.MouseButtonPressed += AppWindowOnMouseButtonPressed;
            this.MouseButtonReleased += AppWindowOnMouseButtonReleased;
            this.MouseMoved += AppWindowOnMouseMoved;
        }
        protected abstract void CreateControls();
        private void InitializeDrawContexts()
        {
            foreach (SFMLControl control in Controls)
            {
                control.Initialize(ApplicationRuntimeContext.Singleton.SFMLRenderingContext);
            }
        }
        protected virtual void PlayIntro()
        {
            // SoundBuffer buffer = new SoundBuffer(Helpers.ReadBinaryResource("Somewhere2.Assets.Sounds.Intro.wav"));
            // Sound sound = new Sound(buffer);
            // sound.Play();
        }
        #endregion
        
        #region Event Handlers
        private void AppWindowOnMouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == Mouse.Button.Left)
            {
                MoveWindowAnchor = new Vector2i(e.X, e.Y);
                MoveWindow = true;
            }
        }
        private void AppWindowOnMouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
            if (MoveWindow) MoveWindow = false;
        }

        private void AppWindowOnMouseMoved(object sender, MouseMoveEventArgs e)
        {
            if (MoveWindow)
            {
                Vector2i current = new Vector2i(e.X, e.Y);
                this.Position = this.Position + (current - MoveWindowAnchor);
            } 
        }
        #endregion
    }
}