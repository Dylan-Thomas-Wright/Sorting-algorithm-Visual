using Microsoft.VisualBasic.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;
using SharpDX.MediaFoundation;
using Sorting_algorithm_Visual.Enums;
using Sorting_algorithm_Visual.Sorting_Algorithms;
using Sorting_algorithm_Visual.States;
using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace Sorting_algorithm_Visual
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public StateMachine _stateMachine;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth =450;
            _graphics.PreferredBackBufferHeight = 400;
            
            
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            MainMenuState _mainMenuState = new MainMenuState(this, GraphicsDevice, _spriteBatch, _graphics, Window);
            _stateMachine = new StateMachine(_mainMenuState);
        }
       
        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            _stateMachine.Update(gameTime);
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            _stateMachine.Draw(gameTime);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();


            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}