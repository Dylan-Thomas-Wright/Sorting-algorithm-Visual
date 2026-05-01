using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorting_algorithm_Visual
{
    internal class QuickSort : SortingAlgorithm
    {
        private int i;
        private int j;
        private SortState state;
        protected override void OnInitialize()
        {
        }
        public override void SortByStep(GraphicsDevice graphics, int? leftPointer, int? rightPointer)
        {
            if(leftPointer != null && rightPointer != null)
            {
                i = leftPointer.Value;
                j = rightPointer.Value;
                Random rnd = new Random();
                int pivotIndex = rnd.Next(i, j + 1);
                Swap(pivotIndex, j + 1);
            }
            if (IsFinished) return;

            while (i < j)
            {
                ResetColors();
                sprites[i].Color = Color.Red;
                sprites[j].Color = Color.Red;
                if (order == SortOrder.Ascending)
                {
                    if (sprites[i].Hight < sprites[j].Hight)
                    {
                        i++;
                    }
                    else
                    {
                        Swap(i, j);
                        j--;
                    }
                }
                else
                {
                    if (sprites[i].Hight > sprites[j].Hight)
                    {
                        i++;
                    }
                    else
                    {
                        Swap(i, j);
                        j--;
                    }
                }
            }
            if(leftPointer < j )
            {
                SortByStep(graphics, leftPointer, j);
            }
            if(i < rightPointer)
            {
                SortByStep(graphics, j + 1, rightPointer);
            }
        }
    }
}
