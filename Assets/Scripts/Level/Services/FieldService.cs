using System;
using Level.Views;
using UnityEngine;

namespace Level.Services
{
    public class FieldService : MonoBehaviour, IFieldService
    {
        [SerializeField] private Node _cellPrefab;
        [SerializeField] private Vector2Int _fieldSize;

        public INode[,] grid { get; private set; }
        public Vector2Int gridSize => _fieldSize;
        
        public INode start { get; private set; }
        public INode finish { get; private set; }

        public event Action<INode> onNodeClick; 


        private void Awake()
        {
            grid = new INode[_fieldSize.x, _fieldSize.y];
        }
        
        public void SetStart(INode node)
        {
            start = node;
            start.SetStart();
        }

        public void ClearWalkable()
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    if (grid[x, y] == start) continue;
                    if (grid[x, y] == finish) continue;
                    grid[x, y].SetWalkable(true);
                }
            }
        }

        public void SetFinish(INode node)
        {
            finish = node;
            finish.SetFinish();
        }

        public void GenerateField()
        {
            for (int y = 0; y < _fieldSize.y; y++)
            {
                for (int x = 0; x < _fieldSize.x; x++)
                {
                    var node = CreateNode();
                    node.SetCoords(new Vector2Int(x, y));
                    node.transform.position = CoordsToPos(node.coords);
                    grid[x, y] = node;
                    node.onClick += OnNodeClick;
                }
            }
        }

        private void OnNodeClick(Node node)
        {
            Debug.Log($"Click {node.coords}");
            onNodeClick?.Invoke(node);
        }

        public void ClearAllField()
        {
            start = null;
            finish = null;
            
            ClearWalkable();
        }

        public Vector3 CoordsToPos(Vector2Int coords)
        {
            return new Vector3(coords.x, 0.0f, coords.y);
        }

        public Vector2Int PosToCoords(Vector3 pos)
        {
            return new Vector2Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.z));
        }

        private Node CreateNode()
        {
            return Instantiate(_cellPrefab);
        }

        public INode NodeFromPos(Vector3 position)
        {
            var coords = PosToCoords(position);
            return grid[coords.x, coords.y];
        }
    }
}