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
        public PlacableTroopController placableTroopPrefab;

        public CardData(string cardName, TroopAttackType cardType, Sprite cardSprite, int cost, int attack, int health, int range, PlacableTroopController placableTroopPrefab)
        {
            this.cardName = cardName;
            this.cardType = cardType;
            this.cardSprite = cardSprite;
            this.cost = cost;
            this.attack = attack;
            this.health = health;
            this.range = range;
            this.placableTroopPrefab = placableTroopPrefab;
        }
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