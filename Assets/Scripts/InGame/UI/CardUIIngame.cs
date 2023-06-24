﻿using System;
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
            if (Physics.Raycast(ray, out RaycastHit raycast))
            {
                Tile tile;
                if (raycast.collider.TryGetComponent<Tile>(out tile))
                {
                    PlayerController_v2.Instance.PlaceCard(CardData,tile.logicPosition);
                    GameController_v2.Instance.CreateMonster(new MonsterModel()
                    {
                        cardId = CardData.CardId,
                        monsterId = "1",
                        XLogicPosition = tile.logicPosition.x,
                        YLogicPosition = tile.logicPosition.y,
                    });
                }
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
        }
    }
}