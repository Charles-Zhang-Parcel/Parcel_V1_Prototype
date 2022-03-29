using System.IO;
using System.Reflection;
using Parcel.Shared;
using SFML.Graphics;

namespace Parcel.SFMLApplication
{
    public class BasicRenderingInfrastructure
    {
        private Stream DefaultFontAsset { get; set; }
        
        public Font DefaultFont { get; set; }
        
        public static BasicRenderingInfrastructure Setup()
        {
            var fontAsset = Helpers.ReadBinaryResource("Parcel.Assets.Fonts.Roboto.Roboto-Regular.ttf");
            var font = new Font(fontAsset);
            
            BasicRenderingInfrastructure infrastructure = new BasicRenderingInfrastructure()
            {
                DefaultFontAsset = fontAsset,
                DefaultFont = font
            };
            return infrastructure;
        }
    }
}