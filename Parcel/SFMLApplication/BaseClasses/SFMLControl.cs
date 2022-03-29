using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace Parcel.SFMLApplication.BaseClasses
{
    public abstract class SFMLControl
    {
        #region Interface
        /// <summary>
        /// Use constructor to pass in necessary preparation data for initialization
        /// </summary>
        public SFMLControl(SFMLRenderWindow owner)
        {
            Owner = owner;
        }
        /// <summary>
        /// Use initialize to register events, create drawbles and register drawbales
        /// </summary>
        public abstract void Initialize(SFMLRenderingContext context);
        
        public virtual void Draw(RenderWindow owner)
        {
            foreach (Drawable drawable in Drawables)
            {
                owner.Draw(drawable);
            }
        }
        #endregion

        #region Common Members
        public SFMLRenderWindow Owner { get; }
        protected List<SFMLControl> Children { get; } = new List<SFMLControl>();
        protected List<Drawable> Drawables { get; } = new List<Drawable>();
        #endregion

        #region Layout
        public abstract void Transform(Vector2f newPosition);
        /// <param name="newScale">Between 0-1 and more, 1 represents default scale.</param>
        public abstract void Scale(float newScale);
        #endregion

        #region Event Handling Helpers
        /// <summary>
        /// TODO: We need collective transform and local bound, then we can compute the global bound, otherwise we can offload this task to implementation of controls
        /// </summary>
        public abstract bool IsMouseOver(Vector2f mouse);

        public virtual void MouseOver() {}
        #endregion
    }
}