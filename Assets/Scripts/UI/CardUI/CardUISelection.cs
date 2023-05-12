using System.Collections;
using System.Collections.Generic;
using MythicEmpire.Enums;
using MythicEmpire.Manager;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardUISelection : CardBaseUI
{
    
    public void OnPointerClick()
    {
        EventManager.Instance.PostEvent(EventID.SelectedCard,CardData.CardId);
    }
}
