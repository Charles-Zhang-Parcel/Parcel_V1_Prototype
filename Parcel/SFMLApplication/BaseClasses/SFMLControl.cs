using System.Collections.Generic;
using SFML.Graphics;

namespace Parcel.SFMLApplication.BaseClasses
{
    public abstract class SFMLControl
    {
        public SFMLControl()
        {
            Children = new List<SFMLControl>();
        }
        public abstract void Initialize(SFMLRenderingContext context);
        public abstract void Draw(RenderWindow owner);
        
        protected List<SFMLControl> Children { get; }
    }
}