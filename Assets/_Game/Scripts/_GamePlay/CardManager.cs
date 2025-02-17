using System;
using System.Collections;
using System.Collections.Generic;
using Arixen.ScriptSmith;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Yuddham
{
    public class CardManager : MonoBehaviour
    {
        [SerializeField] private CardController cardPrefab;
        [SerializeField] private RectTransform _cardsPanel;

        [SerializeField] private RectTransform _previewCardParent;
        [SerializeField] private Sprite defaultCardSprite;

        private GameObject _previewHolder;
        private CardController _previewCard;

        #region Testing purpose, Remove all after testing

        private CardData _cardData;

        #endregion

        private void Awake()
        {
            _previewHolder = new GameObject("PreviewHolder");
            _cardData = new CardData("Brute", TroopAttackType.Attackers, defaultCardSprite, 2, 5, 5, 1, null);
        }

        private void Start()
        {
            StartGame();
        }

        public async void StartGame()
        {
            LoggerService.Debug("Start: " +DateTime.Now);

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
            cardController.SetCardData(_cardData);
            LoggerService.Debug("Created Preview Card: " + cardController.SiblingIndex, LoggerService.LogLevel.Log);
            return cardController;
        }

        private void OnCardUp(CardController card)
        {
            LoggerService.Debug("OnCardUp: " + card.SiblingIndex);
            
        }

        private void OnCardDown(CardController card)
        {
            LoggerService.Debug("OnCardDown: " + card.SiblingIndex);
        }

        private void OnCardDragged(CardController card, Vector2 positionDelta)
        {
            LoggerService.Debug("OnCardDragged: " + card.SiblingIndex+" delta: "+positionDelta);
            card.transform.Translate(positionDelta);
        }
    }
}