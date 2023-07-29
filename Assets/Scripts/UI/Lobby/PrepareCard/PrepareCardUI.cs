using System;
using System.Collections;
using System.Collections.Generic;
using MythicEmpire.Card;
using MythicEmpire.Enums;
using MythicEmpire.Manager;
using MythicEmpire.Manager.MythicEmpire.Manager;
using MythicEmpire.Model;
using MythicEmpire.Networking;
using Newtonsoft.Json.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VContainer;

namespace MythicEmpire.UI.Lobby
{
    public class PrepareCardUI : MonoBehaviour
    {
        [Header("UI")] [Inject] private CardManager _cardManager;
        [Inject] private UserModel _userModel;
        [Inject] private IRealtimeCommunication _realtimeCommunication;
        [SerializeField] private CardBaseUI prefabs;
        [SerializeField] private Transform parents;
        [SerializeField] private ModeGame mode;
        private List<string> _listRender;
        private List<CardBaseUI> _listItems;

        [FormerlySerializedAs("_listSlot")] [Header("Handle action")] [SerializeField]
        private List<CardSlot> _listCardSlot;

        [SerializeField] private GameObject _framePanel;
        [SerializeField] private GameObject _waitingPanel;

        public TMP_Text timeTxt;
        private void HandleListCardSelected(object card)
        {
            var cardSelected = (CardUIDrag)card;
            foreach (var slot in _listCardSlot)
            {
                if (slot.transform.childCount == 0)
                {
                    _listItems.Remove(cardSelected);
                    slot.SetParentFor(cardSelected);
                    return;
                }
            }
        }

        private void Awake()
        {
            _listItems = new List<CardBaseUI>();
            mainAction = new List<Action>();
        }

        private List<Action> mainAction;
        private void Update()
        {
            if (mainAction.Count > 0)
            {
                mainAction[0].Invoke();
                mainAction.RemoveAt(0);
            }
        }
        
        private void Start()
        {
            EventManager.Instance.RegisterListener(EventID.PrepareListCard, HandleListCardSelected);
            EventManager.Instance.RegisterListener(EventID.DeselectCardPrepare, HandleDeselected);
            EventManager.Instance.RegisterListener(EventID.ServerReceiveMatchMaking, time =>
            {
                if ((int)time == 0)
                {
                    mainAction.Add((() =>
                    {
                        _framePanel.SetActive(false);
                        _waitingPanel.SetActive(true);
                    }));
                }
                mainAction.Add(()=>
                {
                    TimeSpan timeSpan = TimeSpan.FromMinutes((int)time);
                    string formattedTime = timeSpan.ToString(@"hh\:mm");
                    timeTxt.text = formattedTime;
                });
            });
            EventManager.Instance.RegisterListener(EventID.CancelMatchMakingSuccess, o =>
            {
                mainAction.Add((() =>
                {
                    _framePanel.SetActive(true);
                    _waitingPanel.SetActive(false);
                }));
            });
            EventManager.Instance.RegisterListener(EventID.OnStartGame, (o) =>
            {
                mainAction.Add(()=> SceneManager.LoadSceneAsync("Game"));
            });
        }

        IEnumerator EnableWaitingPanel(bool active)
        {
            yield return null;
            _framePanel.SetActive(!active);
            _waitingPanel.SetActive(active);
        }

        private void HandleDeselected(object card)
        {
            var cardDeselected = (CardUIDrag)card;
            cardDeselected.transform.SetParent(parents);
            _listItems.Add(cardDeselected);
        }

        private void OnEnable()
        {
            _framePanel.SetActive(true);
            _waitingPanel.SetActive(false);
            _listRender = _userModel.cardListID;
            foreach (var slot in _listCardSlot)
            {
                if (slot.transform.childCount == 1)
                {
                    Destroy(slot.transform.GetChild(0).gameObject);
                    
                }
            }
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

            // foreach (var card in GetListCard())
            // {
            //     Debug.Log(card);
            // }

            _realtimeCommunication.MatchMakingRequest(GetListCard(), mode);
            
        }

        public void ButtonCancelClick()
        {
            _realtimeCommunication.CancelMatchMakingRequest();
        }
    }
}


