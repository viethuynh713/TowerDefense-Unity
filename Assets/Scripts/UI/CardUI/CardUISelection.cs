using System.Collections;
using System.Collections.Generic;
using MythicEmpire.Enums;
using MythicEmpire.Manager;
using MythicEmpire.Manager.MythicEmpire.Manager;
using MythicEmpire.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MythicEmpire.UI
{


    public class CardUISelection : CardBaseUI
    {

        public void OnPointerClick()
        {
            EventManager.Instance.PostEvent(EventID.SelectedCard, CardData.CardId);
        }
    }
}
