using System;
using System.Collections;
using System.Collections.Generic;
using Arixen.ScriptSmith;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Yuddham
{
    public class CardManager : MonoBehaviour
    {
        [SerializeField] private CardController cardPrefab;

        [SerializeField] private RectTransform _cardsPanel;
        [SerializeField] private RectTransform _previewCardParent;
        [SerializeField] private LayerMask _placableAreaLayerMask;

        private PreviewTroopHandler _previewHolder;
        private CardController _previewCard;
        private PlacableTroopController _currentSelectedTroop;

        private Camera _mainCamera;

        #region Testing purpose, Remove all after testing

        [SerializeField] private CardData _cardData;
        private CardData _mockCardData;

        #endregion

        private void Awake()
        {
            _previewHolder = new GameObject("PreviewHolder").AddComponent<PreviewTroopHandler>();
            _mockCardData = new CardData("Brute", TroopAttackType.Attackers, null, 2, 5, 5, 1, null);
            _mainCamera = Camera.main;
        }

        private void Start()
        {
            StartGame();
        }

        public async void StartGame()
        {
            LoggerService.Debug("Start: " + DateTime.Now);

            _previewCard = await CreatePreviewCard();
            for (int i = 0; i < 4; i++)
            {
                _previewCard = await PreviewToCardDashBoard().ContinueWith(CreatePreviewCard);
            }

            LoggerService.Debug("End: " + DateTime.Now);
        }

        private async UniTask PreviewToCardDashBoard()
        {
            if (_previewCard != null)
            {
                _previewCard.transform.SetParent(_cardsPanel);
                _previewCard.CardRectTransform.localScale = Vector3.one;
                _previewCard.transform.SetAsLastSibling();
                _previewCard.SiblingIndex = _previewCard.CardRectTransform.GetSiblingIndex();
                _previewCard.onCardDragged += OnCardDragged;
                _previewCard.onCardDown += OnCardDown;
                _previewCard.onCardUp += OnCardUp;
                LoggerService.Debug("Preview to Card: " + _previewCard.SiblingIndex);
            }
        }

        private async UniTask<CardController> CreatePreviewCard()
        {
            CardController cardController = Instantiate(cardPrefab, _previewCardParent.transform);
            cardController.CardRectTransform.localScale = Vector3.one * .7f;
            cardController.CardRectTransform.localPosition = Vector3.zero;
            cardController.SetCardData(_cardData ?? _mockCardData);
            LoggerService.Debug("Created Preview Card: " + cardController.SiblingIndex);
            return cardController;
        }

        private void OnCardDown(CardController card)
        {
            LoggerService.Debug("OnCardDown: " + card.SiblingIndex);
            _currentSelectedTroop = Instantiate(card.cardData.placableTroopPrefab ?? _cardData.placableTroopPrefab,
                _previewHolder.transform);
            _previewHolder.SetCurrentTroop(_currentSelectedTroop);
        }

        private void OnCardDragged(CardController card, Vector2 positionDelta)
        {
            LoggerService.Debug("OnCardDragged: " + card.SiblingIndex + " delta: " + positionDelta);
            card.transform.Translate(positionDelta);

            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            bool canPlace = false;
            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, _placableAreaLayerMask) &&
                hit.transform.TryGetComponent(out GridCube gridCube))
            {
                EventBusService.InvokeEvent(new HighlightGridCubeEvent(gridCube));
                _previewHolder.transform.position = hit.point;
                canPlace = true;
            }
            else
            {
            }

            card.SetCardState(!canPlace);
            _previewHolder.gameObject.SetActive(canPlace);
        }

        private void OnCardUp(CardController card)
        {
            LoggerService.Debug("OnCardUp: " + card.SiblingIndex);
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, _placableAreaLayerMask))
            {
                _previewHolder.CurrentSelectedTroop.InitTroop(hit.point, new Vector3(-2.179163f,-4.621179f,-1.134742f));
            }
            else
            {
                card.ReturnToCardDashBoard();
                
            }

            EventBusService.InvokeEvent(new HighlightGridCubeEvent(null));
        }
    }
}