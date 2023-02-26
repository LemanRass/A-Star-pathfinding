using Utils.StateMachineTool;

namespace Level.States
{
    public class GenerateLevelState : State<Core>
    {
        public GenerateLevelState(Core core) : base(core) { }

        public override void OnEnter()
        {
            _core.fieldService.GenerateField();
            ChangeState(new PrepareFieldState(_core));
        }
    }
}
