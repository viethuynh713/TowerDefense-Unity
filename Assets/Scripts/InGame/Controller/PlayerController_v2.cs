using System;
using System.Collections.Generic;
using MythicEmpire.Card;
using MythicEmpire.Enums;
using MythicEmpire.Manager.MythicEmpire.Manager;
using MythicEmpire.Model;
using MythicEmpire.Networking;
using MythicEmpire.Networking.Model;
using Networking_System.Model;
using Networking_System.Model.Data.DataReceive;
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
        

        private int energy = 100;

        private ModeGame _modeGame;

        public void Start()
        {
            EventManager.Instance.RegisterListener(EventID.UpdateEnergy, UpdateEnergy);
            EventManager.Instance.RegisterListener(EventID.UpgradeTower, (o)=>isUpdate = false);
            EventManager.Instance.RegisterListener(EventID.SellTower, (o)=>isUpdate = false);
        }


        private void Awake()
        {
            Instance = this;
        }

        private bool isUpdate = false;
        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !isUpdate)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit raycast))
                {
                    Tower tower;
                    if (raycast.collider.TryGetComponent<Tower>(out tower))
                    {
                        if (tower.OwnerId == _userModel.userId)
                        {
                            isUpdate = true;

                            tower.ActiveUI();
                        }
                    }
                }
            }

            if (Input.GetMouseButtonDown(1) && isUpdate)
            {
                isUpdate = false;
            }
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

        public void CastleTakeDamage(CastleTakeDamageData data)
        {
            _realtimeCommunication.CastleTakeDamage(data);
        }
        
        public void UpdateMonsterHp(MonsterTakeDamageData data)
        {
            _realtimeCommunication.UpdateMonsterHp(data);
        }

        public void UpdateMonsterPosition(UpdateMonsterPositionData data)
        {
            _realtimeCommunication.UpdatePosition(data);
        }

        public void SellTower(SellTowerData data)
        {
            _realtimeCommunication.SellTowerRequest(data);
        }

        public  void UpgradeTower(UpgradeTowerData data)
        {
            _realtimeCommunication.UpgradeTower(data);
        }

        public void GainEnergy(AddEnergyData data)
        {
            if (data.ownerId != _userModel.userId) return;
            _realtimeCommunication.AddEnergy(data);
        }

        public void QuitGame()
        {
            _realtimeCommunication.QuitGame();
        }
    }

}