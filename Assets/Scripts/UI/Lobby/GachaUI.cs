using System.Collections;
using System.Collections.Generic;
using MythicEmpire.Enums;
using MythicEmpire.Networking;
using UnityEngine;
using VContainer;

namespace MythicEmpire.UI.Lobby
{
    public class GachaUI : MonoBehaviour
    {
        [Inject] private ICardServiceNetwork _cardServiceNetwork;

        public void BuyGachaButtonClick(int gachaType)
        {
            GachaType type = (GachaType)gachaType;
            _cardServiceNetwork.BuyGachaRequest(type);
        }
    }
}
