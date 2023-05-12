
    using System.Collections.Generic;
    using MythicEmpire.Enums;

    namespace MythicEmpire.Card
    {

        public interface IGetCardInfos
        {
            CardInfo GetCardById(string id);
            List<CardInfo> GetMultiCard();
            List<CardInfo> GetMultiCard(List<string> id);
            List<CardInfo> GetMultiCard(CardType cardType);
            List<CardInfo> GetMultiCard(RarityCard rarityCard);
            List<CardInfo> GetMultiCard(RarityCard rarityCard, CardType cardType);


        }
    }
