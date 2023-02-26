using System.Collections;
using UnityEngine;
using Utils.StateMachineTool;

namespace Level.States
{
    public class CalculatePathState : State<Core>
    {
        private Coroutine _loopCoroutine;
        
        public CalculatePathState(Core core) : base(core) { }

        public override async void OnEnter()
        {
            var pathNodes = await _core.pathfindingService.FindPath(_core.fieldService);
            if (pathNodes == null)
            {
                Debug.LogError("There is no possible path");
                return;
            }

            for (int i = 0; i < pathNodes.Count - 1; i++)
            {
                pathNodes[i].SetAsAPath();
            }

            _loopCoroutine = _core.StartCoroutine(Loop());
        }

        public override void OnExit()
        {
            if (_loopCoroutine != null) _core.StopCoroutine(_loopCoroutine);
        }

        private void Restart()
        {
            ChangeState(new PrepareFieldState(_core));
        }
        
        private IEnumerator Loop()
        {
            while (true)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    yield return null;
                    Restart();
                }
                
                yield return null;
            }
        }
    }
}