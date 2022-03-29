using Parcel.SFMLApplication.BaseClasses;
using SFML.Graphics;
using SFML.System;

namespace Parcel.SFMLApplication.Controls
{
    public class Button: SFMLControl
    {
        public Button(string label)
        {
            Label = label;
        }

        #region Properties
        public string Label { get; set; }
        #endregion
        
        #region Components
        private Text Text { get; set; }
        private RectangleShape Shape { get; set; }
        #endregion

        #region Interface

        public override void Initialize(SFMLRenderingContext context)
        {
            Text = new Text(Label, context.BasicRendering.DefaultFont);
            Text.CharacterSize = 24;
            Text.FillColor = Color.Blue;
            FloatRect bounds = Text.GetGlobalBounds();
                
            Shape = new RectangleShape(new Vector2f(bounds.Width, Text.CharacterSize));
        }

        public override void Draw(RenderWindow owner)
        {
            owner.Draw(Shape);
            owner.Draw(Text);
        }
        #endregion
    }
}