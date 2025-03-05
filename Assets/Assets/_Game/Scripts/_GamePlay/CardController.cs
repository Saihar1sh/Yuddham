using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Yuddham
{
    public class CardController : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
    {
        [SerializeField] private Image _cardDisplayImage;
        [SerializeField] private CanvasGroup _visualsCanvasGroup;

        [SerializeField] private Sprite defaultCardSprite;

        private Vector3 _cardStartPosition;
        
        public CardData cardData { get; private set; }

        public RectTransform CardRectTransform { get; private set; }
        public int SiblingIndex { get; set; }

        public Action<CardController, Vector2> onCardDragged;
        public Action<CardController> onCardDown;
        public Action<CardController> onCardUp;


        public void SetCardData(CardData cardData)
        {
            this.cardData = cardData;
            _cardDisplayImage.sprite = cardData.cardSprite?? defaultCardSprite;
            _cardStartPosition = transform.position;
        }

        public void SetCardState(bool isActive)
        {
            _visualsCanvasGroup.alpha = isActive ? 0.5f : 0;
        }

        public void ReturnToCardDashBoard()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(CardRectTransform.parent as RectTransform);
            _visualsCanvasGroup.alpha = 1;
        }

        #region Unity Callbacks

        private void Awake()
        {
            CardRectTransform = transform as RectTransform;
        }

        public void OnDrag(PointerEventData eventData)
        {
            onCardDragged?.Invoke(this, eventData.delta);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            onCardUp?.Invoke(this);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            onCardDown?.Invoke(this);
        }

        #endregion
    }
}