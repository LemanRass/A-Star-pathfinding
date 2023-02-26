using Level.Services;
using Level.States;
using UnityEngine;
using Utils.StateMachineTool;

namespace Level
{
    public class Core : MonoBehaviour
    {
        [Header("Services")]
        [SerializeField] private FieldService _fieldService;
        public IFieldService fieldService => _fieldService;

        [SerializeField] private PathfindingService _pathfindingService;
        public IPathfindingService pathfindingService => _pathfindingService;
        
        
        private StateMachine<Core> _stateMachine;

        private void Start()
        {
            _stateMachine = new StateMachine<Core>(new GenerateLevelState(this));
        }
    }
}