using System;
using Parcel.SFMLApplication.BaseClasses;
using SFML.Graphics;
using SFML.System;

namespace Parcel.SFMLApplication.Controls
{
    public class Button: SFMLControl
    {
        public Button(SFMLRenderWindow owner, string label)
            :base(owner)
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
            // Create drawables
            Text = new Text(Label, context.BasicRendering.DefaultFont);
            Text.CharacterSize = 24;
            Text.FillColor = Color.Blue;
            FloatRect bounds = Text.GetGlobalBounds();

            Shape = new RectangleShape(new Vector2f(bounds.Width, Text.CharacterSize));

            // Register interaction events
            
            // Register drawables
            Drawables.Add(Shape);
            Drawables.Add(Text);
        }
        #endregion
        
        #region Layouts
        public override void Transform(Vector2f newPosition)
        {
            throw new NotImplementedException();
        }

        public override void Scale(float newScale)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Event Handling
        public override bool IsMouseOver(Vector2f mouse)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}