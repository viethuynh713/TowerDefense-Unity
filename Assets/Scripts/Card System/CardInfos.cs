
using System;
using MythicEmpire.Enums;
using UnityEngine;

namespace MythicEmpire.Card
{


    [Serializable]
    public class CardInfo
    {
        public string CardId;
        public string CardName;
        public int CardStart;
        public Sprite CardImage;
        public CardType TypeOfCard;
        public RarityCard CardRarity;
        public StatsCard CardStats;
    }
}