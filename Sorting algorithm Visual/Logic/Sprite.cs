using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace Sorting_algorithm_Visual
{
    internal class Sprite
    {
         public Texture2D Texture;
         public Vector2 Position;
        public Vector2 TargetPosition;
         public int Hight;
        public Color Color = Color.White;
        public Rectangle Rectangle { get { return new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Hight); } }
        public Sprite(Texture2D texture, Vector2 position, int hight)
         {
                Texture = texture;
                Position = position;
                Hight = hight;
        }
        public void Update(GameTime gameTime)
        {
            Position += (TargetPosition - Position) * 0.2f;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rectangle, Color);
        }
    }
}
