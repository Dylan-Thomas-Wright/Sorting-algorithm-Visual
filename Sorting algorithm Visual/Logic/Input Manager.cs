using Microsoft.Xna.Framework.Input;

namespace Sorting_algorithm_Visual
{
    public class InputManager
    {
        private static KeyboardState current;
        private static KeyboardState previous;
        private static MouseState currentMouse;
        private static MouseState previousMouse;
        public InputManager()
        {

        }
        public void Update()
        {
            previous = current;
            current = Keyboard.GetState();
            previousMouse = currentMouse;
            currentMouse = Mouse.GetState();
        }

        public bool IsMouseButtonPressed(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return currentMouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton == ButtonState.Released;
                case MouseButton.Right:
                    return currentMouse.RightButton == ButtonState.Pressed && previousMouse.RightButton == ButtonState.Released;
                default:
                    return false;
            }
        }
        public bool IsMouseButtonDown(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return currentMouse.LeftButton == ButtonState.Pressed;
                case MouseButton.Right:
                    return currentMouse.RightButton == ButtonState.Pressed;
                default:
                    return false;
            }
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
    public enum MouseButton
    {
        Left,
        Right
    }
}
