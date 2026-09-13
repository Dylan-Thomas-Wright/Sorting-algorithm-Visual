using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Sorting_algorithm_Visual
{
    public class Button : Sprite
    {
        public bool isHovering = false;
        public bool isClicked = false;

        public EventHandler Click;
        public Button(Texture2D texture, Vector2 position, int height) : base(texture, position, height)
        {
        }

        public void Update(GameTime gameTime, InputManager inputManager)
        {
            var mouseState = Mouse.GetState();
            Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);
            isHovering = Rectangle.Contains(mousePosition);
            if (isHovering && inputManager.IsMouseButtonPressed(MouseButton.Left) && !isClicked)
            {
                isClicked = true;
                Click?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                isClicked = false;
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Color HoveringColor = new Color (Color.A/2, Color.G/2, Color.B);
            Color currentColor = isHovering ? Color : HoveringColor; 
            spriteBatch.Draw(Texture, Rectangle, currentColor);
        }
    }
}
