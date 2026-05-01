using Microsoft.VisualBasic.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;
using Sorting_algorithm_Visual.Enums;
using Sorting_algorithm_Visual.Sorting_Algorithms;
using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace Sorting_algorithm_Visual
{
    public class Game1 : Game
    {
        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(
        IntPtr hWnd,
        IntPtr hWndInsertAfter,
        int X, int Y, int cx, int cy,
        uint uFlags
    );

        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private const uint SWP_NOMOVE = 0x0002;
        private const uint SWP_NOSIZE = 0x0001;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private InputManager _inputManager= new InputManager();
        private SortingAlgorithm _currentAlgorithm;
        private Sprite[] sprites;
        private int[] values;

        private int listSize = 20;
        private bool sorting = false;
        private bool scrambleing = false;

        private float timer = 0f;
        private float delay = 0.01f;
        private float scrambleDelay = 0.5f;
        private bool AutoSort = false;
        private bool IsScrambled = true;
        private bool StepByStep = false;

        private SortOrder _order = SortOrder.Ascending;
        private AlgorithmType _algorithm;

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
            GenerateNumbers(); 
            Texture2D texture = new Texture2D(GraphicsDevice, 15, 1); 
            Color[] data = new Color[15];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = Color.White;
            }
            texture.SetData(data);
            sprites = new Sprite[values.Length];

            int screenWidth = GraphicsDevice.Viewport.Width;
            int screenHeight = GraphicsDevice.Viewport.Height;
            int spacing = screenWidth / values.Length;

            int maxValue = values.Max();
            float heightScale;
            if (maxValue > 0)
            {
                heightScale = (float)screenHeight / maxValue;
            }
            else
            {
                heightScale = 1f;
            }
            int startX = (screenWidth - (spacing * values.Length)) / 2;
            for (int i = 0; i < sprites.Length; i++) 
            {
                int height = (int)(values[i] * heightScale);
                int startY = (GraphicsDevice.Viewport.Height - height); 
                sprites[i] = new Sprite(texture, new Vector2(startX + i * spacing, startY), height); 
                sprites[i].TargetPosition = sprites[i].Position; } 
        }
        protected override void Initialize()
        {
            base.Initialize();
            MakeTopMost();
        }
        private void MakeTopMost()
        {
            if (Window != null && Window.Handle != IntPtr.Zero)
            {
                SetWindowPos(
                    Window.Handle,
                    HWND_TOPMOST,
                    0, 0, 0, 0,
                    SWP_NOMOVE | SWP_NOSIZE
                );
            }
        }
        private void GenerateNumbers()
        {
            values = new int[listSize];
            Random rand = new Random();
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = rand.Next(1, int.MaxValue);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            HandleInput();
            HandleSorting(gameTime);
            HandleScrambling(gameTime);
            
            foreach(Sprite sprite in sprites)
            {
                sprite.Update(gameTime);
            }

            base.Update(gameTime);
        }
        private void HandleInput()
        {
            _inputManager.Update();
            if (_inputManager.IsKeyPressed(Keys.Escape))
                Exit();

            if (_inputManager.IsKeyPressed(Keys.U))
            {
                AutoSort = !AutoSort;
            }

            if (AutoSort)
            {
                HandleAutoSort();
                return;
            }

            if (StepByStep && sorting)
            {
                sorting = false;
            }

            if (_inputManager.IsKeyPressed(Keys.Space) && StepByStep)
            {
                sorting = true;
            }

            if (_inputManager.IsKeyPressed(Keys.A) && !sorting)
            {
                _order = SortOrder.Ascending;
            }

            if (_inputManager.IsKeyPressed(Keys.D) && !sorting)
            {
                _order = SortOrder.Descending;
            }
            if (_inputManager.IsKeyPressed(Keys.L))
            {
                if (StepByStep)
                {
                    sorting = true;
                }
                StepByStep = !StepByStep;
            }
            if (_inputManager.IsKeyDown(Keys.S) && !sorting && !scrambleing)
            {
                StartScramble();
            }
            if (_inputManager.IsKeyDown(Keys.B) && !sorting)
            {
                StartAlgorithm(AlgorithmType.BubbleSort);
            }

            if (_inputManager.IsKeyDown(Keys.I) && !sorting)
            {
                StartAlgorithm(AlgorithmType.InsertionSort);
            }
            if(_inputManager.IsKeyDown(Keys.Q) && !sorting)
            {
                StartAlgorithm(AlgorithmType.QuickSort);
            }
        }
        private void HandleAutoSort()
        {
            if (sorting) return;

            if (!IsScrambled)
            {
                StartScramble();
            }
            else
            {
                Random rand = new Random();

                _algorithm = (AlgorithmType)rand.Next(Enum.GetValues(typeof(AlgorithmType)).Length);
                _order = (SortOrder)rand.Next(Enum.GetValues(typeof(SortOrder)).Length);

                StartAlgorithm(_algorithm);
            }
        }
        private void StartAlgorithm(AlgorithmType type)
        {
            _algorithm = type;

            _currentAlgorithm = type switch
            {
                AlgorithmType.BubbleSort => new BubbleSort(),
                AlgorithmType.InsertionSort => new InterstionSort(),
                AlgorithmType.QuickSort => new QuickSort(),
                _ => null
            };

            _currentAlgorithm?.Initialize(sprites, _order);

            sorting = true;
        }
        private void HandleSorting(GameTime gameTime)
        {
            if (!sorting || _currentAlgorithm == null)
                return;

            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer >= delay)
            {
                _currentAlgorithm.SortByStep(GraphicsDevice);
                timer = 0f;

                if (_currentAlgorithm.IsFinished)
                {
                    sorting = false;
                    IsScrambled = false;
                }
            }
        }

        private void StartScramble()
        {
            scrambleing = true;
            IsScrambled = true;
            foreach (Sprite sprite in sprites)
            {
                sprite.Color = Color.White;
            }
            Random rand = new Random();

            for (int i = sprites.Length - 1; i > 0; i--)
            {
                int j = rand.Next(i + 1);

                var temp = sprites[i];
                sprites[i] = sprites[j];
                sprites[j] = temp;
            }

            UpdateTargets(GraphicsDevice);
        }
        private void HandleScrambling(GameTime gameTime)
        {
            if (!scrambleing) return;

            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer >= scrambleDelay)
            {
                scrambleing = false;
                timer = 0f;
            }
        }
        protected void Swap(int a, int b)
        {
            Sprite temp = sprites[a];
            sprites[a] = sprites[b];
            sprites[b] = temp;
        }

        private void ScrambleSprites()
        {
            foreach (Sprite sprite in sprites)
            {
                sprite.Color = Color.White;
            }

            Random rand = new Random();
            for (int i = sprites.Length - 1; i > 0; i--)
            {
                int j = rand.Next(0, i + 1);
                Swap(i, j);
            }
            UpdateTargets(GraphicsDevice);
        }
        public void UpdateTargets(GraphicsDevice GraphicsDevice)
        {
            int screenWidth = GraphicsDevice.Viewport.Width;
            int screenHeight = GraphicsDevice.Viewport.Height;

            int count = sprites.Length;

            if (count == 0)
            {
                return;
            }
            int spacing = screenWidth / count;

            int startX = (screenWidth - (spacing * count)) / 2;

            for (int i = 0; i < count; i++)
            {
                int startY = screenHeight - sprites[i].Hight;

                sprites[i].TargetPosition = new Vector2(
                    startX + i * spacing,
                    startY
                );
            }
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            foreach (Sprite sprite in sprites)
            {
                sprite.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}