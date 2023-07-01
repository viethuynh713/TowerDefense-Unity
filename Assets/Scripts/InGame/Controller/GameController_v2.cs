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
            // StartCoroutine(GetInfo());
            mainThreadAction = new List<Action>();
            _realtimeCommunication.GetCard();
            _realtimeCommunication.GetMap();
            // EventManager.Instance.RegisterListener(EventID.PlaceCard, (o) =>
            // {
            //     JObject package = (JObject)o;
            //     CardType type = (CardType)((int)package["cardType"]);
            //     var data = (string)package["cardData"];
            //     switch (type)
            //     {
            //         case CardType.MonsterCard:
            //             mainThreadAction.Add(()=>CreateMonster(JsonConvert.DeserializeObject<MonsterModel>(data)));
            //             break;
            //         case CardType.TowerCard:
            //             mainThreadAction.Add(()=>BuildTower(JsonConvert.DeserializeObject<TowerModel>(data)));
            //             break;
            //         case CardType.SpellCard:
            //             mainThreadAction.Add(()=>PlaceSpell(JsonConvert.DeserializeObject<SpellModel>(data)));
            //             break;
            //
            //     }
            // });
            EventManager.Instance.RegisterListener(EventID.CreateMonster, o => mainThreadAction.Add(()=>CreateMonster((MonsterModel)o)));
            EventManager.Instance.RegisterListener(EventID.BuildTower, o => mainThreadAction.Add(()=>BuildTower((TowerModel)o)));
            EventManager.Instance.RegisterListener(EventID.PlaceSpell, o => mainThreadAction.Add(()=>PlaceSpell((SpellModel)o)));
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
    }
}