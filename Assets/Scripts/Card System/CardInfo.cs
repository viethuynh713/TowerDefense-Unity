
using System;
using MythicEmpire.Enums;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Serialization;

namespace MythicEmpire.Card
{


    [CreateAssetMenu(fileName = "Card Data", menuName = "MythicEmpire Data/Data/Card")]
    
    public class CardInfo : ScriptableObject
    {
        public string CardId = Guid.NewGuid().ToString();
        public string CardName;
        [Range(0,5)]
        public int CardStar;
        [JsonIgnore]
        public Sprite CardImage;
        public CardType TypeOfCard;
        public RarityCard CardRarity;
        [JsonIgnore]
        public StatsCard CardStats;
        
    }
}