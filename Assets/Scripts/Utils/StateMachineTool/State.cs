namespace Utils.StateMachineTool
{
    public abstract class State<T>
    {
        protected readonly T _core;
        private StateMachine<T> _stateMachine;

        protected State(T core) => _core = core;

        public void Init(StateMachine<T> stateMachine)
            => _stateMachine = stateMachine;
        
        public virtual void OnEnter() {}
        public virtual void OnExit() {}

        protected void ChangeState(State<T> state)
            => _stateMachine.ChangeState(state);
    }
}