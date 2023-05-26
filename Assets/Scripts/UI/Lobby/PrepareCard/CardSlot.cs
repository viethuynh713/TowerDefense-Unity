using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace MythicEmpire.UI.Lobby
{
public class CardSlot : MonoBehaviour, IDropHandler
{
    
    public void OnDrop(PointerEventData eventData)
    {
         Debug.Log("Drop");
        if (transform.childCount == 0)
        {
            CardUIDrag item = eventData.pointerDrag.GetComponent<CardUIDrag>();
            
            if (!item) return;
            item.parentAfterDrag = transform;
        }
    }

    public void SetParentFor(CardUIDrag item)
    {
        item.transform.SetParent(this.transform);
    }
}
}
