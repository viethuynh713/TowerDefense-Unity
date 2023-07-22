using System;
using MythicEmpire.Card;
using MythicEmpire.Networking.Model;
using MythicEmpire.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MythicEmpire.InGame
{
    public class CardUIIngame : CardBaseUI, IBeginDragHandler, IEndDragHandler, IDragHandler
    {

        public void OnBeginDrag(PointerEventData eventData)
        {
            
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycast,1000,LayerMask.GetMask("Map")))
            {
                Tile tile;
                if (raycast.collider.TryGetComponent<Tile>(out tile))
                {
                    PlayerController_v2.Instance.PlaceCard(CardData,tile.logicPosition);
                }
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
        }
    }
}