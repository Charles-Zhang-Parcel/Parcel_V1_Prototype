using Parcel.SFMLApplication.BaseClasses;
using SFML.Graphics;
using SFML.System;

namespace Parcel.SFMLApplication.Controls
{
    public class Node: SFMLControl
    {
        public Node(SFMLRenderWindow owner, string label)
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
            Owner.RegisterMouseEventListener(this);
            
            // Register drawables
            Drawables.Add(Shape);
            Drawables.Add(Text);
        }
        #endregion

        #region Layouts
        public override void Transform(Vector2f newPosition)
        {
            throw new System.NotImplementedException();
        }
        public override void Scale(float newScale)
        {
            throw new System.NotImplementedException();
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