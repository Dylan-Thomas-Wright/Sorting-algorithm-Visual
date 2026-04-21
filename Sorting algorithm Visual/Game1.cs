using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Sorting_algorithm_Visual
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Sprite[] sprites;
        private int[] values = new int[26] { 5, 3, 8, 1, 2, 7, 4, 6, 9, 10, 7, 6, 2, 11, 13, 22, 23,56, 43, 4, 53, 72, 36, 28, 40, 3 };

        private int i = 0;
        private int j = 0;
        private bool sorting = false;

        private float timer = 0f;
        private float delay = 0.01f;

        private int CurrentIndex = -1;
        private int NextIndex = -1;
        private int sortedIndex = -1;

        private enum SortState
        {
            Comparing,
            Swapping
        }

        private enum SortAlgorithm
        {
            BubbleSort,
            InsertionSort
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
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Texture2D texture = new Texture2D(GraphicsDevice, 15, 1);
            Color[] data = new Color[15];
            for (int i = 0; i < data.Length; i++)
                data[i] = Color.White;

            texture.SetData(data);

            sprites = new Sprite[values.Length];

            int spacing = 30;
            int startX = (GraphicsDevice.Viewport.Width - values.Length * spacing) / 2;

            for (int i = 0; i < sprites.Length; i++)
            {
                int Height = values[i] * 10;
                sprites[i] = new Sprite(
                    texture,
                    new Vector2(startX + i * spacing, 450 - Height),
                    Height
                );

                sprites[i].TargetPosition = sprites[i].Position;
            }
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState BoardState = Keyboard.GetState();

            if (BoardState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            if(BoardState.IsKeyDown(Keys.A) && !sorting)
            {
                _order = SortOrder.Ascending;
            }
            if (BoardState.IsKeyDown(Keys.D) && !sorting)
            {
                _order = SortOrder.Descending;
            }
            if(BoardState.IsKeyDown(Keys.S) && !sorting)
            {
                ScrambleSprites();
            }
            if (BoardState.IsKeyDown(Keys.B) && !sorting)
            {
                StartSorting();
                _algorithm = SortAlgorithm.BubbleSort; 
            }
            if (BoardState.IsKeyDown(Keys.I) && !sorting)
            {
                StartSorting();
                _algorithm = SortAlgorithm.InsertionSort;
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
                    }
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
        }
        private bool CheckEndSort()
        {
            if (i >= sprites.Length)
            {
                sorting = false;

                foreach (Sprite sprite in sprites)
                {
                    sprite.Color = Color.Green;
                }

                return true;
            }
            return false;
        }
        private void ResetColors()
        {
            foreach (Sprite sprite in sprites)
            {
                sprite.Color = Color.White;
            }
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
            int spacing = 30;
            int startX = (GraphicsDevice.Viewport.Width - sprites.Length * spacing) / 2;

            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i].TargetPosition = new Vector2(startX + i * spacing, 450 - sprites[i].Hight);
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