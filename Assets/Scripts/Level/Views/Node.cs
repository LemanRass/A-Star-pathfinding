using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Level.Views
{
    public class Node : MonoBehaviour, INode, IPointerClickHandler
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private TextMeshProUGUI _costsLabel;
    
        public bool isWalkable { get; private set; }
        public Vector2Int coords { get; private set; }

        private int _hCost;

        public int hCost
        {
            get => _hCost;

            set
            {
                _hCost = value;
                _costsLabel.text = $"G: {_gCost}\n" +
                                   $"H: {_hCost}\n" +
                                   $"F: {fCost}";
            }
        }

        private int _gCost;
        public int gCost
        {
            get => _gCost;
            set
            {
                _gCost = value;
                _costsLabel.text = $"G: {_gCost}\n" +
                                   $"H: {_hCost}\n" +
                                   $"F: {fCost}";
            }
        }
        public int fCost => gCost + hCost;
        public INode parent { get; set; }

        public event Action<Node> onClick; 


        private void Awake()
        {
            var matCopy = new Material(_renderer.sharedMaterial);
            _renderer.sharedMaterial = matCopy;
            SetWalkable(true);
        }

        public void SetStart()
        {
            _renderer.sharedMaterial.color = Color.green;
        }

        public void SetFinish()
        {
            _renderer.sharedMaterial.color = Color.red;
        }
    
        public void SetCoords(Vector2Int coords)
        {
            this.coords = coords;
        }
    
        public void SetWalkable(bool walkable)
        {
            isWalkable = walkable;
            _renderer.sharedMaterial.color = isWalkable ? Color.white : Color.black;
        }

        public void SetAsAPath()
        {
            _renderer.sharedMaterial.color = Color.yellow;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            onClick?.Invoke(this);
        }
    }
}