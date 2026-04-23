using Microsoft.VisualBasic.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;
using System;
using System.Runtime.InteropServices;
using System.Linq;

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
        private KeyboardState _previousKeyboardState;
        private KeyboardState _currentKeyboardState;

        private Sprite[] sprites;
        private int[] values;

        private int i = 0;
        private int j = 0;
        private int listSize = 20;
        private bool sorting = false;
        private bool scrambleing = false;

        private float timer = 0f;
        private float delay = 0.01f;
        private float scrambleDelay = 0.5f;

        private int CurrentIndex = -1;
        private int NextIndex = -1;
        private int sortedIndex = -1;

        private bool AutoSort = false;
        private bool IsScrambled = true;
        private bool StepByStep = false;

        private bool partitioning = false;
        private int LeftPointer = -1;
        private int RightPointer = -1;
        private int PivotIndex = -1;

        private enum SortState
        {
            Comparing,
            Swapping
        }

        private enum SortAlgorithm
        {
            BubbleSort,
            InsertionSort,
            QuickSort
        }

        private enum SortOrder
        {
            Ascending,
            Descending
        }

        private SortOrder _order = SortOrder.Ascending;
        private SortAlgorithm _algorithm;

        private SortState state = SortState.Comparing;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth =1000;
            
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
            _previousKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();

            if (_currentKeyboardState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            if (AutoSort)
            {
                if (!sorting)
                {
                    if (!IsScrambled)
                    {
                        ScrambleSprites();
                        IsScrambled = true;
                        scrambleing = true;
                    }
                    else
                    {
                        Random rand = new Random();
                        _algorithm = (SortAlgorithm)rand.Next(0, Enum.GetValues(typeof(SortAlgorithm)).Length);
                        _order = (SortOrder)rand.Next(0, Enum.GetValues(typeof(SortOrder)).Length);
                        StartSorting();
                    }
                }
            }
            else
            {
                if (StepByStep)
                {
                    if (sorting)
                    {
                        sorting = false;
                    }
                    if(_currentKeyboardState.IsKeyDown(Keys.Space) && !_previousKeyboardState.IsKeyDown(Keys.Space) && !sorting)
                    {
                        sorting = true;
                    }
                }
                if (_currentKeyboardState.IsKeyDown(Keys.A) && !_previousKeyboardState.IsKeyDown(Keys.A) && !sorting)
                {
                    _order = SortOrder.Ascending;
                }
                if (_currentKeyboardState.IsKeyDown(Keys.D) && !_previousKeyboardState.IsKeyDown(Keys.D) && !sorting)
                {
                    _order = SortOrder.Descending;
                }
                if (_currentKeyboardState.IsKeyDown(Keys.L) && !_previousKeyboardState.IsKeyDown(Keys.L))
                {
                    if (StepByStep)
                    {
                        sorting = true;
                    }
                    StepByStep = !StepByStep;
                }
                if (_currentKeyboardState.IsKeyDown(Keys.S)&&!sorting && !scrambleing)
                {
                    ScrambleSprites();
                    scrambleing = true;
                    IsScrambled = true;
                }
                if (_currentKeyboardState.IsKeyDown(Keys.B) && !sorting)
                {
                    StartSorting();
                    _algorithm = SortAlgorithm.BubbleSort;
                }
                if (_currentKeyboardState.IsKeyDown(Keys.I) && !sorting)
                {
                    StartSorting();
                    _algorithm = SortAlgorithm.InsertionSort;
                }
                if(_currentKeyboardState.IsKeyDown(Keys.Q) && !sorting)
                {
                    StartQuickSort(sprites);
                    _algorithm = SortAlgorithm.QuickSort;
                }
            }
            if(_currentKeyboardState.IsKeyDown(Keys.U) && !_previousKeyboardState.IsKeyDown(Keys.U))
            {
                AutoSort = !AutoSort;
            }

            // Run step with delay
            if (sorting)
            {
                timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (timer >= delay)
                {
                    switch (_algorithm)
                    {
                        case SortAlgorithm.BubbleSort:
                            BubbleSortStep();
                            break;
                        case SortAlgorithm.InsertionSort:
                            InterstionSort();
                            break;
                        case SortAlgorithm.QuickSort:
                            QuickSortStep(sprites);
                            break;
                    }
                    timer = 0f;
                }
            }
            if(scrambleing)
            {
                timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (timer >= scrambleDelay)
                {
                    scrambleing = false;
                    timer = 0f;
                }
            }

            foreach (Sprite sprite in sprites)
            {
                sprite.Update(gameTime);
            }

            base.Update(gameTime);
        }
        private void InterstionSort()
        {
            ResetColors();

            bool endSort = CheckEndSort();
            if (endSort) return;

            CurrentIndex = i;
            NextIndex = i - 1;

            HighlightSorted();


            sprites[CurrentIndex].Color = Color.Red;
            if (NextIndex >= 0)
            {
                sprites[NextIndex].Color = Color.Orange;
            }

            if (state == SortState.Comparing)
            {
                bool SwapNeeded = false;
                switch (_order)
                {
                    case SortOrder.Ascending:
                        SwapNeeded = j > 0 && sprites[j].Hight < sprites[j - 1].Hight;
                        break;
                    case SortOrder.Descending:
                        SwapNeeded = j > 0 && sprites[j].Hight > sprites[j - 1].Hight;
                        break;
                }

                if (SwapNeeded)
                {
                    state = SortState.Swapping;
                }
                else
                {
                    i++;
                    j = i;
                }
            }
            else if (state == SortState.Swapping)
            {
                sprites[j].Color = Color.Green;
                sprites[j - 1].Color = Color.Green;
                Swap(j, j - 1);
                j--;
                UpdateTargets();
                state = SortState.Comparing;
            }
        }
        private void BubbleSortStep()
        {
            ResetColors();

            bool endSort =CheckEndSort();
            if (endSort) return;

            CurrentIndex = j;
            NextIndex = j + 1;

            HighlightSorted();

            sprites[CurrentIndex].Color = Color.Red;
            sprites[NextIndex].Color = Color.Orange;

            if (state == SortState.Comparing)
            {
                bool SwapNeeded = false;
                switch (_order)
                {
                    case SortOrder.Ascending:
                        SwapNeeded = sprites[j].Hight > sprites[j + 1].Hight;
                        break;
                    case SortOrder.Descending:
                        SwapNeeded = sprites[j].Hight < sprites[j + 1].Hight;
                        break;
                }
                if (SwapNeeded)
                {
                    state = SortState.Swapping;
                }
                else
                {
                    NextBubbleStep();
                }
            }
            else if (state == SortState.Swapping)
            {
                sprites[j].Color = Color.Green;
                sprites[j + 1].Color = Color.Green;

                Swap(j, j + 1);
                UpdateTargets();

                state = SortState.Comparing;
                NextBubbleStep();
            }
        }

        private void NextBubbleStep()
        {
            j++;

            if (j >= sprites.Length - i - 1)
            {
                j = 0;
                i++;
                sortedIndex = sprites.Length - i;
            }
        }
        private void QuickSortStep(Sprite[] sprites)
        {
            sorting = false;
        }

        private void Swap(int a, int b)
        {
            Sprite temp = sprites[a];
            sprites[a] = sprites[b];
            sprites[b] = temp;
        }
        private void HighlightSorted()
        {
            for (int k = sprites.Length - 1; k >= sortedIndex; k--)
            {
                sprites[k].Color = Color.Green;
            }
            IsScrambled = false;
        }
        private bool CheckEndSort()
        {
            if (i >= sprites.Length)
            {
                EndSort();

                return true;
            }
            return false;
        }
        private void EndSort()
        {
            sorting = false;
            foreach (Sprite sprite in sprites)
            {
                sprite.Color = Color.Green;
            }
        }
        private void ResetColors()
        {
            foreach (Sprite sprite in sprites)
            {
                sprite.Color = Color.White;
            }
        }
        private void StartQuickSort(Sprite[] sprites)
        {
            sorting = true;
            LeftPointer = 0;
            RightPointer = sprites.Length - 1;
            partitioning = false;
            i = LeftPointer;
        }
        private void StartSorting()
        {
            sorting = true;
            i = 0;
            j = 0;
            sortedIndex = sprites.Length;
            state = SortState.Comparing;
        }
        private void UpdateTargets()
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

        private void ScrambleSprites()
        {
            ResetColors();
            Random rand = new Random();
            for (int i = sprites.Length - 1; i > 0; i--)
            {
                int j = rand.Next(0, i + 1);
                Swap(i, j);
            }
            UpdateTargets();
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