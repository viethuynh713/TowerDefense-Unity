using System;
using System.Collections;
using System.Collections.Generic;
using MythicEmpire.Card;
using MythicEmpire.Enums;
using MythicEmpire.Manager.MythicEmpire.Manager;
using MythicEmpire.Networking;
using UnityEngine;
using VContainer;

namespace MythicEmpire.UI.Lobby
{
    public class GachaUI : MonoBehaviour
    {
        [Inject] private ICardServiceNetwork _cardServiceNetwork;
        [SerializeField] private CollectCardUI _collectCard;

        private void Start()
        {
            _collectCard.gameObject.SetActive(false);
            EventManager.Instance.RegisterListener(EventID.OnBuyGachaSuccess,HandleGachaSuccess);
        }

        private void HandleGachaSuccess(object obj)
        {
            _collectCard.gameObject.SetActive(true);
            _collectCard.SetCard((CardInfo)obj);
        }

        public void BuyGachaButtonClick(int gachaType)
        {
            GachaType type = (GachaType)gachaType;
            _cardServiceNetwork.BuyGachaRequest(type);
        }
    }
}
