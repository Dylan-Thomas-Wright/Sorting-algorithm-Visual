using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;
using System.Windows.Forms;

namespace Sorting_algorithm_Visual
{
    abstract class SortingAlgorithm
    {
        protected Sprite[] sprites;

        public bool IsFinished { get; protected set; }
        protected SortOrder order;

        public void Initialize(Sprite[] sprites, SortOrder order)
        {
            this.sprites = sprites;
            this.order = order;
            IsFinished = false;
            OnInitialize();
        }

        protected abstract void OnInitialize();
        public abstract void SortByStep(GraphicsDevice graphics);

        protected void Swap(int a, int b)
        {
            Sprite temp = sprites[a];
            sprites[a] = sprites[b];
            sprites[b] = temp;
        }

        protected void EndSort()
        {
            IsFinished = true;

            foreach (Sprite sprite in sprites)
            {
                sprite.Color = Color.Green;
            }
        }

        protected void ResetColors()
        {
            foreach (Sprite sprite in sprites)
            {
                sprite.Color = Color.White;
            }
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

    }
}
