using System;
using Arixen.ScriptSmith;
using UnityEngine;

namespace Yuddham
{
    public class GridCube : MonoBehaviour
    {
        [SerializeField] private Sprite _highlightedSprite;
        
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public Vector2Int Index { get; private set; }

        private void Awake()
        {
            EventBusService.Subscribe<HighlightGridCubeEvent>(HighlightCube);
        }

        private void OnDestroy()
        {
            EventBusService.UnSubscribe<HighlightGridCubeEvent>(HighlightCube);
        }

        private void Start()
        {
            _spriteRenderer.gameObject.SetActive(false);
        }

        public void Init(Vector2Int index)
        {
            Index = index;
        }

        private void HighlightCube(HighlightGridCubeEvent e)
        {
            _spriteRenderer.gameObject.SetActive(e._selectedCube == this);
        }
    }

   
}
