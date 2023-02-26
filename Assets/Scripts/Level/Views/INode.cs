using System;
using UnityEngine;

namespace Level.Views
{
    public interface INode
    {
        public bool isWalkable { get; }
        public Vector2Int coords { get; }
        public int hCost { get; set; }
        public int gCost { get; set; }
        public int fCost => gCost + hCost;
        public INode parent { get; set; }

        public void SetStart();
        public void SetFinish();
        public void SetCoords(Vector2Int coords);
        public void SetWalkable(bool walkable);
        public void SetAsAPath();
    }
}