using SFML.Graphics;

namespace Parcel.SFMLApplication
{
    public class SFMLRenderingContext
    {
        #region Rendering Windows
        public RenderWindow MainWindow { get; set; }
        #endregion

        #region Rendering Resources
        public BasicRenderingInfrastructure BasicRendering { get; set; }
        #endregion
    }
}