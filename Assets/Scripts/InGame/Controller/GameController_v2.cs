﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MythicEmpire.Card;
using MythicEmpire.Enums;
using MythicEmpire.Manager.MythicEmpire.Manager;
using MythicEmpire.Model;
using MythicEmpire.Networking;
using MythicEmpire.Networking.Model;
using Unity.Mathematics;
using UnityEngine;
using VContainer;

namespace MythicEmpire.InGame
{
    public class GameController_v2 : MonoBehaviour
    {
        public static GameController_v2 Instance;
        [SerializeField]public MapService_v2 mapService;
        [Inject] private IRealtimeCommunication _realtimeCommunication;
        [Inject] private UserModel _userModel;
        [Inject] private CardManager _cardManager;

        public List<Action> mainThreadAction;

        private List<Tower> _towers;
        public void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _towers = new List<Tower>();
            mainThreadAction = new List<Action>();
            _realtimeCommunication.GetGameInfo();

            EventManager.Instance.RegisterListener(EventID.CreateMonster, o => mainThreadAction.Add(()=>CreateMonster((MonsterModel)o)));
            EventManager.Instance.RegisterListener(EventID.BuildTower, o => mainThreadAction.Add(()=>BuildTower((TowerModel)o)));
            EventManager.Instance.RegisterListener(EventID.PlaceSpell, o => mainThreadAction.Add(()=>PlaceSpell((SpellModel)o)));
            EventManager.Instance.RegisterListener(EventID.SpawnWave, o => mainThreadAction.Add(()=>SpawnWave((MonsterModel)o)));
            EventManager.Instance.RegisterListener(EventID.SellTower, o => mainThreadAction.Add(()=>SellTower((TowerModel)o)));
            EventManager.Instance.RegisterListener(EventID.UpgradeTower, o => mainThreadAction.Add(()=>UpgradeTower((UpgradeTowerDataSender)o)));
        }

        private void UpgradeTower(UpgradeTowerDataSender upgradeTowerDataSender)
        {
            var tower = _towers.FirstOrDefault(t => t.TowerID == upgradeTowerDataSender.towerId);

            if (tower != null) tower.Upgrade(upgradeTowerDataSender);
        }

        private void SellTower(TowerModel towerModel)
        {
            var tower = _towers.FirstOrDefault(tower => tower.TowerID == towerModel.towerId);
            if (tower != null)
            {
                tower.Sell();
                mapService.ReleaseTile(towerModel.XLogicPosition, towerModel.YLogicPosition);
            }
            
        }

        private void Update()
        {
            if (mainThreadAction.Count > 0)
            {
                mainThreadAction[0].Invoke();
                mainThreadAction.RemoveAt(0);
            }
        }
        public void BuildTower(TowerModel data)
        {
            var cardInfo = _cardManager.GetCardById(data.cardId);
            var tower = Instantiate(cardInfo.GameObjectPrefab, new Vector3(data.XLogicPosition, 0.3f, data.YLogicPosition),
                quaternion.identity);
            tower.GetComponent<Tower>().Init(data.towerId,data.ownerId,(TowerStats)cardInfo.CardStats);
            
            _towers.Add(tower.GetComponent<Tower>());
            
            mapService.BanPosition(data.XLogicPosition, data.YLogicPosition);
            
        }

        public void CreateMonster(MonsterModel data)
        {
            var cardInfo = _cardManager.GetCardById(data.cardId);
            var monster = Instantiate(cardInfo.GameObjectPrefab, new Vector3(data.XLogicPosition, 0f, data.YLogicPosition),
                quaternion.identity);
            // TODO: create new stats. -> Done

            monster.GetComponent<Monster>().Init(data.monsterId,data.ownerId,true,(MonsterStats)cardInfo.CardStats,((MonsterStats)(cardInfo.CardStats)).Hp, _userModel.userId == data.ownerId);
        }

        public void PlaceSpell(SpellModel data)
        {
            var cardInfo = _cardManager.GetCardById(data.cardId);
            var spell = Instantiate(cardInfo.GameObjectPrefab, new Vector3(data.XLogicPosition, 0, data.YLogicPosition),
                quaternion.identity);
            spell.GetComponent<Spell>().Init(data.spellId,data.ownerId,(SpellStats)cardInfo.CardStats);
            Debug.Log(spell.name);

        }

        public void SpawnWave(MonsterModel data)
        {
            // Debug.Log(data.monsterId);
            var cardInfo = _cardManager.GetCardById(data.cardId);
            var monster = Instantiate(cardInfo.GameObjectPrefab, new Vector3(data.XLogicPosition, 0, data.YLogicPosition),
                quaternion.identity);
            monster.GetComponent<Monster>().Init(data.monsterId,data.ownerId,false,(MonsterStats)cardInfo.CardStats, data.maxHp,_userModel.userId == data.ownerId);
        }
    }
}