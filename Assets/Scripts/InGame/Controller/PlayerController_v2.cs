using System;
using MythicEmpire.Card;
using MythicEmpire.Enums;
using MythicEmpire.Manager.MythicEmpire.Manager;
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
            if (cardData.CardStats.Energy > energy) return;
            if (cardData.TypeOfCard != CardType.SpellCard)
            {
                // if (!_mapService.IsValidPosition(placePosition, _userModel.userId)) return;
                if (!_mapService.IsValidPosition(placePosition, "1")) return;
            }

            PlaceCardData data = new PlaceCardData()
            {
                cardId = cardData.CardId,
                typeOfCard = cardData.TypeOfCard,
                Xposition = placePosition.x,
                Yposition = placePosition.y,
                stats = cardData.CardStats
            };
            _realtimeCommunication.PlaceCardRequest(data);
        }
    }
}