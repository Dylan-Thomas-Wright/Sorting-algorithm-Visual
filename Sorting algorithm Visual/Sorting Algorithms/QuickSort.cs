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
        private Stack<(int, int)> stack;
        private int low;
        private int high;
        private int pivotIndex;
        private SortState state;
        protected override void OnInitialize()
        {
        }
        public override void SortByStep(GraphicsDevice graphics)
        {
            IsFinished = true;
        }
        private void Partition(int low, int high)
        {
        }
    }
}
