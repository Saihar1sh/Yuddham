using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yuddham
{
    [CreateAssetMenu(fileName = "NewCardData", menuName = "ScriptableObjects/CardData", order = 1)]
    public class CardData : ScriptableObject
    {
        public string cardName;
        public TroopAttackType cardType;
        public Sprite cardSprite;
        public int cost;
        public int attack;
        public int health;
        public int range;
        public CardController cardPrefab;
        public PlacableTroopController placableTroopPrefab;
    }

    [Flags]
    public enum TroopAttackType
    {
        None = 0,
        Buildings = 1,
        Attackers = 2,
        AirForce = 4,
        LongRange = 8,
        Support = 16,
        Siege = 32
    }
}