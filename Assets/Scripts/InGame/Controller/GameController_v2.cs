using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using MythicEmpire.Card;
using MythicEmpire.Enums;
using MythicEmpire.Manager.MythicEmpire.Manager;
using MythicEmpire.Model;
using MythicEmpire.Networking;
using MythicEmpire.Networking.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Unity.Mathematics;
using UnityEngine;
using VContainer;

namespace MythicEmpire.InGame
{
    public class GameController_v2 : MonoBehaviour
    {
        public static GameController_v2 Instance;
        [SerializeField]private MapService_v2 _mapService;
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
            Instantiate(cardInfo.GameObjectPrefab, new Vector3(data.XLogicPosition, 0, data.YLogicPosition),
                quaternion.identity);
            _mapService.BanPosition(data.XLogicPosition, data.YLogicPosition);
            
        }

        public void CreateMonster(MonsterModel data)
        {
            var cardInfo = _cardManager.GetCardById(data.cardId);
            Instantiate(cardInfo.GameObjectPrefab, new Vector3(data.XLogicPosition, 0, data.YLogicPosition),
                quaternion.identity);
        }

        public void PlaceSpell(SpellModel data)
        {
            var cardInfo = _cardManager.GetCardById(data.cardId);
            Instantiate(cardInfo.GameObjectPrefab, new Vector3(data.XLogicPosition, 0, data.YLogicPosition),
                quaternion.identity);
        }

        public IEnumerator SpawnWave(List<string> cards)
        {
            Debug.Log(cards.Count);
            yield return null;
        }
    }
}