using Utils.StateMachineTool;

namespace Level.States
{
    public class PrepareFieldState : State<Core>
    {
        public PrepareFieldState(Core core) : base(core) { }

        public override void OnEnter()
        {
            _core.fieldService.ClearAllField();
            ChangeState(new SelectStartState(_core));
        }
    }
}