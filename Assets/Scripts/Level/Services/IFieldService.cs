using System;
using Level.Views;
using UnityEngine;

namespace Level.Services
{
    public interface IFieldService
    {
        public INode[,] grid { get; }
        public Vector2Int gridSize { get; }
        public INode start { get; }
        public INode finish { get; }
        
        public event Action<INode> onNodeClick; 

        public void SetStart(INode node);
        public void SetFinish(INode node);
        public void GenerateField();
        public void ClearWalkable();
        public void ClearAllField();
        public Vector3 CoordsToPos(Vector2Int coords);
        public Vector2Int PosToCoords(Vector3 pos);
        public INode NodeFromPos(Vector3 position);
    }
}