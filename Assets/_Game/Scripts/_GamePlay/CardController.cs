using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class CardController : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public RectTransform CardRectTransform { get; private set; }

    public Action<CardController, Vector2> onCardDragged;
    public Action<CardController> onCardDown;
    public Action<CardController> onCardUp;

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
}