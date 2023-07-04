using System;
using MythicEmpire.Card;
using MythicEmpire.Enums;
using MythicEmpire.Manager.MythicEmpire.Manager;
using MythicEmpire.Model;
using MythicEmpire.Networking;
using MythicEmpire.Networking.Model;
using UnityEngine;
using VContainer;

namespace MythicEmpire.InGame
{
    public class PlayerController_v2 : MonoBehaviour
    {
        public static PlayerController_v2 Instance;
        [SerializeField] private MapService_v2 _mapService;
        [Inject] private IRealtimeCommunication _realtimeCommunication;
        [Inject] private UserModel _userModel;

        public int energy = 100;

        public void Start()
        {
            EventManager.Instance.RegisterListener(EventID.UpdateEnergy, UpdateEnergy);
        }


        private void Awake()
        {
            Instance = this;
        }

        private void UpdateEnergy(object newEnergy)
        {
            energy = (int)newEnergy;
        }

        public void PlaceCard(CardInfo cardData, Vector2Int placePosition)
        {
            // Debug.Log(energy);
            if (cardData.CardStats.Energy > energy) return;
            if (cardData.TypeOfCard != CardType.SpellCard)
            {
                if (!_mapService.IsValidPosition(placePosition, _userModel.userId)) return;
            }

            switch (cardData.TypeOfCard)
            {
                case CardType.MonsterCard:
                    MonsterStats monsterStats = (MonsterStats)cardData.CardStats;
                    CreateMonsterData createMonsterData = new CreateMonsterData()
                    {
                        cardId = cardData.CardId,
                        Xposition = placePosition.x,
                        Yposition = placePosition.y,
                        stats = monsterStats
                    };
                    _realtimeCommunication.CreateMonsterRequest(createMonsterData);
                    break;
                case CardType.TowerCard:
                    TowerStats towerStats = (TowerStats)cardData.CardStats;
                    BuildTowerData buildTowerData = new BuildTowerData()
                    {
                        cardId = cardData.CardId,
                        Xposition = placePosition.x,
                        Yposition = placePosition.y,
                        stats = towerStats
                    };
                    _realtimeCommunication.BuildTowerRequest(buildTowerData);
                    break;
                case CardType.SpellCard:
                    SpellStats spellStats = (SpellStats)cardData.CardStats;
                    PlaceSpellData placeSpellData = new PlaceSpellData()
                    {
                        cardId = cardData.CardId,
                        Xposition = placePosition.x,
                        Yposition = placePosition.y,
                        stats = spellStats
                    };
                    _realtimeCommunication.PlaceSpellRequest(placeSpellData);
                    break;
            }
        }
    }

}