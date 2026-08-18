using Microsoft.VisualBasic.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Sorting_algorithm_Visual.States
{
    internal abstract class State
    {
        protected Game1 Game { get; }
        protected SpriteBatch SpriteBatch { get; }

        protected State(Game1 game, SpriteBatch spriteBatch)
        {
            Game = game;
            SpriteBatch = spriteBatch;
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);
    }
}
