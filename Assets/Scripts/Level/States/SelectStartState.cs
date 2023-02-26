using Level.Views;
using UnityEngine;
using Utils.StateMachineTool;

namespace Level.States
{
    public class SelectStartState : State<Core>
    {
        public SelectStartState(Core core) : base(core) { }

        public override void OnEnter()
        {
            _core.fieldService.onNodeClick += OnNodeClick;
            Debug.Log("SelectStartState -> Enter");
        }

        public override void OnExit()
        {
            _core.fieldService.onNodeClick -= OnNodeClick;
            Debug.Log("SelectStartState -> Exit");
        }

        private void OnNodeClick(INode node)
        {
            _core.fieldService.SetStart(node);
            ChangeState(new SelectFinishState(_core));
        }
    }
}