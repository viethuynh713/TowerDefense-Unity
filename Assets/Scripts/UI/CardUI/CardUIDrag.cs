using System.Collections;
using System.Collections.Generic;
using MythicEmpire.Enums;
using MythicEmpire.Manager;
using MythicEmpire.Manager.MythicEmpire.Manager;
using MythicEmpire.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MythicEmpire.UI
{
    public class CardUIDrag : CardBaseUI,    IPointerClickHandler
    {
        [Header("Drag Item")] [SerializeField] private Image _image;
        [HideInInspector] public Transform parentAfterDrag;
        private bool isDrag = false;

        // public void OnBeginDrag(PointerEventData eventData)
        // {
        //     _image.raycastTarget = false;
        //     parentAfterDrag = transform.parent;
        //     transform.SetParent(transform.root);
        //     isDrag = true;
        // }
        //
        // public void OnDrag(PointerEventData eventData)
        // {
        //     transform.position = Input.mousePosition;
        // }
        //
        // public void OnEndDrag(PointerEventData eventData)
        // {
        //     _image.raycastTarget = true;
        //     transform.SetParent(parentAfterDrag);
        //     isDrag = false;
        //
        // }

        public void OnPointerClick(PointerEventData eventData)
        {
             Debug.Log(eventData.pointerId);
            if (!isDrag && eventData.pointerId == -1)
            {
                EventManager.Instance.PostEvent(EventID.PrepareListCard, this);
            }            
            if (!isDrag && eventData.pointerId == -2)
            {
                EventManager.Instance.PostEvent(EventID.DeselectCardPrepare, this);
            }
        }
    }
}
