using System;
using System.Collections;
using System.Collections.Generic;
using MythicEmpire.Card;
using MythicEmpire.Enums;
using MythicEmpire.Manager;
using MythicEmpire.Manager.MythicEmpire.Manager;
using MythicEmpire.Model;
using Newtonsoft.Json.Serialization;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VContainer;

namespace MythicEmpire.UI.Lobby
{
    public class PrepareCardUI : MonoBehaviour
    {
        [Header("UI")] [Inject] private CardManager _cardManager;
        [Inject] private UserModel _userModel;
        [SerializeField] private CardBaseUI prefabs;
        [SerializeField] private Transform parents;
        [SerializeField] private ModeGame mode;
        private List<string> _listRender;
        private List<CardBaseUI> _listItems;

        [FormerlySerializedAs("_listSlot")] [Header("Handle action")] [SerializeField]
        private List<CardSlot> _listCardSlot;


        private void HandleListCardSelected(object card)
        {
            var cardSelected = (CardUIDrag)card;
            foreach (var slot in _listCardSlot)
            {
                if (slot.transform.childCount == 0)
                {
                    slot.SetParentFor(cardSelected);
                    return;
                }
            }
        }

        private void Awake()
        {
            _listItems = new List<CardBaseUI>();
        }

        private void Start()
        {
            EventManager.Instance.RegisterListener(EventID.PrepareListCard, HandleListCardSelected);
        }

        private void OnEnable()
        {
            _listRender = _userModel.cardListID;
            if (_listItems != null)
            {
                foreach (var item in _listItems)
                {
                    Destroy(item.gameObject);
                }

            }

            _listItems = new List<CardBaseUI>();
            foreach (var card in _cardManager.GetMultiCard(_userModel.cardListID))
            {
                var cardUi = Instantiate(prefabs, parents);
                cardUi.SetUI(card);
                _listItems.Add(cardUi);
            }
        }

        public void FilterTowerCard(Toggle toggle)
        {
            if (!toggle.isOn) return;
            _listItems.ForEach(card => card.gameObject.SetActive(card.CardData.TypeOfCard == CardType.TowerCard));
        }

        public void FilterMonsterCard(Toggle toggle)
        {
            if (!toggle.isOn) return;
            _listItems.ForEach(card => card.gameObject.SetActive(card.CardData.TypeOfCard == CardType.MonsterCard));
        }

        public void FilterSpellCard(Toggle toggle)
        {
            if (!toggle.isOn) return;
            _listItems.ForEach(card => card.gameObject.SetActive(card.CardData.TypeOfCard == CardType.SpellCard));
        }

        public void FilterAllCard(Toggle toggle)
        {
            if (!toggle.isOn) return;
            _listItems.ForEach(card => card.gameObject.SetActive(true));
        }

        public void OpenPreparePnlCardButtonClick(int modeGame)
        {
            if (!Enum.IsDefined(typeof(ModeGame), mode)) return;
            mode = (ModeGame)modeGame;
            gameObject.SetActive(true);
        }

        private List<string> GetListCard()
        {
            List<string> cardSelected = new List<string>();
            foreach (var slot in _listCardSlot)
            {
                if (slot.transform.childCount == 1)
                {
                    var card = slot.transform.GetChild(0).GetComponent<CardUIDrag>();
                    if (card != null)
                    {
                        cardSelected.Add(card.CardData.CardId);
                    }
                }
            }

            return cardSelected;
        }

        public void ButtonReadyClick()
        {

            foreach (var card in GetListCard())
            {
                Debug.Log(card);
            }
            // Debug.Log(  GetListCard().ToString());
        }
    }
}


