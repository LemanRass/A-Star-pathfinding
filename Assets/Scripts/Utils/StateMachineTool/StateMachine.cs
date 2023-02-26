namespace Utils.StateMachineTool
{
    public class StateMachine<T>
    {
        private State<T> _currentState;

        public StateMachine(State<T> state) => ChangeState(state);

        public void ChangeState(State<T> newState)
        {
            _currentState?.OnExit();
            _currentState = newState;
            _currentState.Init(this);
            _currentState?.OnEnter();
        }

        public void Dispose()
        {
            _currentState?.OnExit();
            _currentState = null;
        }
    }
}