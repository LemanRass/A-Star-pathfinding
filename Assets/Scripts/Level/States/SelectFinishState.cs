using Level.Views;
using UnityEngine;
using Utils.StateMachineTool;

namespace Level.States
{
    public class SelectFinishState : State<Core>
    {
        public SelectFinishState(Core core) : base(core) { }

        public override void OnEnter()
        {
            _core.fieldService.onNodeClick += OnNodeClick;
            Debug.Log("SelectFinishState -> Enter");
        }

        public override void OnExit()
        {
            _core.fieldService.onNodeClick -= OnNodeClick;
            Debug.Log("SelectFinishState -> Exit");
        }

        private void OnNodeClick(INode node)
        {
            if (node == _core.fieldService.start) return;
            _core.fieldService.SetFinish(node);
            ChangeState(new SelectUnwalkableState(_core));
        }
    }
}