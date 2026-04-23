using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct2D1.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sorting_algorithm_Visual.Sorting_Algorithms
{
    internal class BubbleSort: SortingAlgorithm
    {
        private int i;
        private int j;
        private int sortedIndex;
        private SortState state;

        protected override void OnInitialize()
        {
            i = 0;
            j = 0;
            sortedIndex = sprites.Length - 1;
            state = SortState.Comparing;
        }
        public override void SortByStep(GraphicsDevice graphics)
        {
            if (i >= sprites.Length)
            {
                EndSort();
                return;
            }
            if (IsFinished)
            {
                return;
            }
            ResetColors();

            for (int k = sprites.Length - 1; k >= sortedIndex; k--)
            {
                sprites[k].Color = Color.Green;
            }

            sprites[j].Color = Color.Red;
            sprites[j + 1].Color = Color.Orange;

            if (state == SortState.Comparing)
            {
                bool SwapNeeded = false;
                switch (order)
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
                    NextStep();
                }
            }
            else if (state == SortState.Swapping)
            {
                sprites[j].Color = Color.Green;
                sprites[j + 1].Color = Color.Green;

                Swap(j, j + 1);
                UpdateTargets(graphics);

                state = SortState.Comparing;
                NextStep();
            }
        }
        private void NextStep()
        {
            j++;

            if (j >= sprites.Length - i - 1)
            {
                j = 0;
                i++;
                sortedIndex = sprites.Length - i;
            }
        }

    }
}
