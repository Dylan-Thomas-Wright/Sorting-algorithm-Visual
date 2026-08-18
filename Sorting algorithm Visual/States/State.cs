using Microsoft.VisualBasic.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Sorting_algorithm_Visual.States
{
    internal interface IState
    {
        public void Enter();
        public void Exit();
        public void Update(GameTime gameTime);
        public void Draw(GameTime gameTime);
    }
}
