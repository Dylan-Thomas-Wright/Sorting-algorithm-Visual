using Microsoft.Xna.Framework;


namespace Sorting_algorithm_Visual.States
{
    internal class StateMachine
    {
        private IState _currentState;

        public StateMachine(IState initialState)
        {
            _currentState = initialState;
            _currentState.Enter();
        }

        public void ChangeState(IState newState)
        {
            _currentState.Exit();
            _currentState = newState;
            _currentState.Enter();
        }

        public void Update(GameTime gameTime)
        {
            _currentState.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            _currentState.Draw(gameTime);
        }
    }
}
