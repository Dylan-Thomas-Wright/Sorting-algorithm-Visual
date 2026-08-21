using Microsoft.VisualBasic.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Sorting_algorithm_Visual.States
{
    internal interface IState
    {
        public Game1 game1 { get; }
        public GraphicsDevice GraphicsDevice { get; }
        public SpriteBatch _spriteBatch { get; }
        public GraphicsDeviceManager GraphicsDeviceManager { get; }

        public GameWindow Window { get; }
        public void Enter();
        public void Exit();
        public void Update(GameTime gameTime);
        public void Draw(GameTime gameTime);
    }
}
