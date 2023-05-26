
using System;
using System.Collections.Generic;
using MythicEmpire.Enums;
using UnityEngine;

namespace MythicEmpire.Card
{

    [CreateAssetMenu(fileName = "New Data Manager", menuName = "MythicEmpire Data/Data Manager")]

    public class CardManager : ScriptableObject, IGetCardInfos
    {
        [SerializeField] public List<CardInfo> ListCards;



        public CardInfo GetCardById(string id)
        {
            return ListCards.Find(card => card.CardId.Equals(id));
        }

        public List<CardInfo> GetMultiCard()
        {
            return ListCards;
        }
        public List<CardInfo> GetMultiCard(List<string> ids)
        {
            List<CardInfo> result = new List<CardInfo>();
            foreach (var id in ids)
            {
                result.Add(GetCardById(id));
            }

            return result;
        }

        public List<CardInfo> GetMultiCard(CardType cardType)
        {
            return ListCards.FindAll(card => card.TypeOfCard.Equals(cardType));
        }

        public List<CardInfo> GetMultiCard(RarityCard rarityCard)
        {
            return ListCards.FindAll(card => card.CardRarity.Equals(rarityCard) && card.CardStar == 0);

        }

        public List<CardInfo> GetMultiCard(RarityCard rarityCard, CardType cardType)
        {
            return ListCards.FindAll(card => card.TypeOfCard.Equals(cardType) && card.CardRarity.Equals(rarityCard));

        }
    }
}
