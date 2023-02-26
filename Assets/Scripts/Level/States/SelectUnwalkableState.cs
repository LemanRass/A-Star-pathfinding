using System.Collections;
using Level.Views;
using Unity.VisualScripting.FullSerializer.Internal.Converters;
using UnityEngine;
using Utils.StateMachineTool;

namespace Level.States
{
    public class SelectUnwalkableState : State<Core>
    {
        private Coroutine _loopCoroutine;
        
        public SelectUnwalkableState(Core core) : base(core) { }

        public override void OnEnter()
        {
            _core.fieldService.onNodeClick += OnNodeClick;
            _loopCoroutine = _core.StartCoroutine(Loop());
            Debug.Log("SelectUnwalkableState -> Enter");
        }

        public override void OnExit()
        {
            if (_loopCoroutine != null) _core.StopCoroutine(_loopCoroutine);
            _core.fieldService.onNodeClick -= OnNodeClick;
            Debug.Log("SelectUnwalkableState -> Exit");
        }

        private IEnumerator Loop()
        {
            while (true)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    yield return null;
                    _core.fieldService.ClearWalkable();
                    GenerateRandom();
                }
                
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    yield return null;
                    FinishEditing();
                }
                
                yield return null;
            }
        }

        private void GenerateRandom()
        {
            for (int y = 0; y < _core.fieldService.grid.GetLength(1); y++)
            {
                for (int x = 0; x < _core.fieldService.grid.GetLength(0); x++)
                {
                    var current = _core.fieldService.grid[x, y];
                    if (current == _core.fieldService.start) continue;
                    if (current == _core.fieldService.finish) continue;
                    current.SetWalkable(Random.Range(0, 3) != 0);
                }
            }
        }

        private void FinishEditing()
        {
            ChangeState(new CalculatePathState(_core));
        }

        private void OnNodeClick(INode node)
        {
            if (node == _core.fieldService.start) return;
            if (node == _core.fieldService.finish) return;
            node.SetWalkable(!node.isWalkable);
        }
    }
}