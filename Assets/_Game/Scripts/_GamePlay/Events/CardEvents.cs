using System.Collections;
using System.Collections.Generic;
using Arixen.ScriptSmith;
using UnityEngine;

namespace Yuddham
{
    public class CardClickEvent : GameEventData
    {
        public CardController cardController;

        public CardClickEvent(CardController cardController)
        {
            this.cardController = cardController;
        }
    }
    
    public class CardDragEvent : GameEventData
    {
        public CardController cardController;

        public CardDragEvent(CardController cardController)
        {
            this.cardController = cardController;
        }
    }
}
