using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sorting_algorithm_Visual.States
{
    internal class MainMenuState:IState
    {
        public Game1 game1 { get; }
        public GraphicsDevice GraphicsDevice { get; }
        public SpriteBatch _spriteBatch { get; }
        public GraphicsDeviceManager GraphicsDeviceManager { get; }
        public GameWindow Window { get; }
        public void Enter()
        {
            // Implement main menu enter logic here
        }

        public void Exit()
        {
            // Implement main menu exit logic here
        }

        public void Update(GameTime gameTime)
        {
            // Implement main menu update logic here
        }

        public void Draw(GameTime gameTime)
        {
            // Implement main menu drawing logic here
        }
    }
}
