using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorting_algorithm_Visual
{
    public class InputManager
    {
        private static KeyboardState current;
        private static KeyboardState previous;
        public InputManager()
        {

        }
        public void Update()
        {
            previous = current;
            current = Keyboard.GetState();
        }

        public bool IsKeyPressed(Keys key)
        {
            return current.IsKeyDown(key) && !previous.IsKeyDown(key);
        }

        public bool IsKeyDown(Keys key)
        {
            return current.IsKeyDown(key);
        }
    }
}
