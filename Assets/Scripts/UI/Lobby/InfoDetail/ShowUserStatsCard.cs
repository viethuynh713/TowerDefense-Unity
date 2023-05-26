using System.Collections.Generic;
using MythicEmpire.Card;
using MythicEmpire.Enums;
using TMPro;
using UnityEngine;

namespace MythicEmpire.UI.Lobby
{
    public class ShowUserStatsCard : MonoBehaviour
    {
        [SerializeField] private TMP_Text _towerCardText;
        [SerializeField] private TMP_Text _monsterCardText;
        [SerializeField] private TMP_Text _spellCardText;
        [SerializeField] private TMP_Text _commonCardText;
        [SerializeField] private TMP_Text _rareCardText;
        [SerializeField] private TMP_Text _mythicCardText;
        [SerializeField] private TMP_Text _legendCardText;
        public void ShowStatCard(List<CardInfo> infos)
        {
            var towerCard = infos.FindAll(x => x.TypeOfCard == CardType.TowerCard).Count;
            _towerCardText.text = towerCard.ToString();
            
            var monsterCard = infos.FindAll(x => x.TypeOfCard == CardType.MonsterCard).Count;
            _monsterCardText.text = monsterCard.ToString();

            var spellCard = infos.FindAll(x => x.TypeOfCard == CardType.SpellCard).Count;
            _spellCardText.text = towerCard.ToString();

            var commonCard = infos.FindAll(x => x.CardRarity == RarityCard.Common).Count;
            _commonCardText.text = commonCard.ToString();
            
            var rareCard = infos.FindAll(x => x.CardRarity == RarityCard.Rare).Count;
            _rareCardText.text = rareCard.ToString();
            
            var mythicCard = infos.FindAll(x => x.CardRarity == RarityCard.Mythic).Count;
            _mythicCardText.text = mythicCard.ToString();
            
            var legendCard = infos.FindAll(x => x.CardRarity == RarityCard.Legend).Count;
            _legendCardText.text = legendCard.ToString();



        }
    }
}