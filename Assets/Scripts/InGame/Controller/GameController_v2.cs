using System;
using System.Collections;
using System.Collections.Generic;
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
        public void Awake()
        {
            Instance = this;
        }

        private void Start()
        {

            mainThreadAction = new List<Action>();
            _realtimeCommunication.GetCard();
            _realtimeCommunication.GetMap();

            EventManager.Instance.RegisterListener(EventID.CreateMonster, o => mainThreadAction.Add(()=>CreateMonster((MonsterModel)o)));
            EventManager.Instance.RegisterListener(EventID.BuildTower, o => mainThreadAction.Add(()=>BuildTower((TowerModel)o)));
            EventManager.Instance.RegisterListener(EventID.PlaceSpell, o => mainThreadAction.Add(()=>PlaceSpell((SpellModel)o)));
            EventManager.Instance.RegisterListener(EventID.SpawnWave, o => mainThreadAction.Add(()=>StartCoroutine(SpawnWave((List<string>)o))));
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
            var tower = Instantiate(cardInfo.GameObjectPrefab, new Vector3(data.XLogicPosition, 0, data.YLogicPosition),
                quaternion.identity);
            tower.GetComponent<Tower>().Init(data.towerId,data.ownerId, new Vector2Int(data.XLogicPosition,data.YLogicPosition),(TowerStats)cardInfo.CardStats);
            mapService.BanPosition(data.XLogicPosition, data.YLogicPosition);
            
        }

        public void CreateMonster(MonsterModel data)
        {
            var cardInfo = _cardManager.GetCardById(data.cardId);
            var monster = Instantiate(cardInfo.GameObjectPrefab, new Vector3(data.XLogicPosition, 0, data.YLogicPosition),
                quaternion.identity);
            monster.GetComponent<Monster>().Init(data.monsterId,data.ownerId,true,(MonsterStats)cardInfo.CardStats, _userModel.userId == data.ownerId);
        }

        public void PlaceSpell(SpellModel data)
        {
            var cardInfo = _cardManager.GetCardById(data.cardId);
            var spell = Instantiate(cardInfo.GameObjectPrefab, new Vector3(data.XLogicPosition, 0, data.YLogicPosition),
                quaternion.identity);
            spell.GetComponent<Spell>().Init(data.spellId,data.ownerId,(SpellStats)cardInfo.CardStats);
        }

        public IEnumerator SpawnWave(List<string> cards)
        {
            Debug.Log(cards.Count);
            yield return null;
        }
    }
}