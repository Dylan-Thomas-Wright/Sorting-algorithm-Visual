using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorting_algorithm_Visual
{
    internal class InterstionSort : SortingAlgorithm
    {
        private int i;
        private int j;
        private SortState state;
        protected override void OnInitialize()
        {
            i = 1;
            j = i;
            state = SortState.Comparing;
        }
        public override void SortByStep(GraphicsDevice graphics)
        {
            if (IsFinished) return;

            if (i >= sprites.Length)
            {
                EndSort();
                return;
            }

            ResetColors();

            for (int k = 0; k < i; k++)
            {
                sprites[k].Color = Color.Green;
            }

            sprites[j].Color = Color.Red;

            if (j > 0)
                sprites[j - 1].Color = Color.Orange;

            if (state == SortState.Comparing)
            {
                bool swapNeeded = false;
                switch(order)
                {
                    case SortOrder.Ascending:
                        swapNeeded = j > 0 && sprites[j].Hight < sprites[j - 1].Hight;
                        break;
                    case SortOrder.Descending:
                            swapNeeded = j > 0 && sprites[j].Hight > sprites[j - 1].Hight;
                        break;
                }
                if (swapNeeded)
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

                state = SortState.Comparing;

                UpdateTargets(graphics); 
            }
        }

    }
}
