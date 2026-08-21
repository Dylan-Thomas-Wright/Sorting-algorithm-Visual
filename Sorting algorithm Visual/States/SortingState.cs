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

namespace Sorting_algorithm_Visual.States
{
    internal class SortingState : IState
    {
        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(
        IntPtr hWnd,
        IntPtr hWndInsertAfter,
        int X, int Y, int cx, int cy,
        uint uFlags
        );

        public Game1 game1 { get; private set; }
        public GraphicsDevice GraphicsDevice { get; private set; }
        public SpriteBatch _spriteBatch { get; private set; }
        public GraphicsDeviceManager GraphicsDeviceManager { get; private set; }
        public GameWindow Window { get; private set; }


        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private const uint SWP_NOMOVE = 0x0002;
        private const uint SWP_NOSIZE = 0x0001;

        private InputManager _inputManager = new InputManager();
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
        public void Enter()
        {
            GenerateRandomNumbers();

            Texture2D texture = GenerateSquareTexture(15);

            CreateSprites(texture);
        }
        private void CreateSprites(Texture2D texture)
        {
            sprites = new Sprite[values.Length];

            int screenWidth = GraphicsDevice.Viewport.Width;
            int screenHeight = GraphicsDevice.Viewport.Height;

            int spacing = CalculateSpacing(screenWidth);
            float heightScale = CalculateHeightScale(screenHeight);
            int startX = CalculateStartX(screenWidth, spacing);

            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i] = CreateSprite(texture, i, startX, spacing, heightScale);
            }
        }
        private Sprite CreateSprite(Texture2D texture, int index, int startX, int spacing, float heightScale)
        {
            int height = (int)(values[index] * heightScale);
            int startY = GraphicsDevice.Viewport.Height - height;

            Sprite sprite = new Sprite(
                texture,
                new Vector2(startX + index * spacing, startY),
                height);

            sprite.TargetPosition = sprite.Position;

            return sprite;
        }
        private int CalculateSpacing(int screenWidth)
        {
            return screenWidth / values.Length;
        }

        private float CalculateHeightScale(int screenHeight)
        {
            int maxValue = values.Max();

            if (maxValue == 0)
                return 1f;

            return (float)screenHeight / maxValue;
        }

        private int CalculateStartX(int screenWidth, int spacing)
        {
            return (screenWidth - (spacing * values.Length)) / 2;
        }
        private Texture2D GenerateSquareTexture(int width)
        {
            Texture2D texture = new Texture2D(GraphicsDevice, width, 1);
            Color[] data = new Color[15];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = Color.White;
            }
            texture.SetData(data);
            return texture;
        }
        private void MakeTheScreenStayAtTop()
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
        private void GenerateRandomNumbers()
        {
            values = new int[listSize];
            Random rand = new Random();
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = rand.Next(1, int.MaxValue);
            }
        }

        public void Exit()
        {
            // Implement main menu exit logic here
        }

        public void Update(GameTime gameTime)
        {
            HandleInput();
            HandleSorting(gameTime);
            HandleScrambling(gameTime);

            foreach (Sprite sprite in sprites)
            {
                sprite.Update(gameTime);
            }
        }
        private void HandleInput()
        {
            _inputManager.Update();
            if (_inputManager.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape))
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
            //if(_inputManager.IsKeyDown(Keys.Q) && !sorting)
            //{
            //    StartAlgorithm(AlgorithmType.QuickSort);
            //}
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
                //AlgorithmType.QuickSort => new QuickSort(),
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

        public void Draw(GameTime gameTime)
        {

            _spriteBatch.Begin();

            foreach (Sprite sprite in sprites)
            {
                sprite.Draw(_spriteBatch);
            }

            _spriteBatch.End();
        }
    }
}
