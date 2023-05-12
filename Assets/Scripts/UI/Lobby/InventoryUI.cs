using System;
using System.Collections;
using System.Collections.Generic;
using MythicEmpire.Card;
using MythicEmpire.Enums;
using MythicEmpire.Manager;
using MythicEmpire.PlayerInfos;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class InventoryUI : MonoBehaviour
{
    [Inject] private CardManager _manager;
    [Inject] private UserModel _userModel;    
    

    [SerializeField] private CardBaseUI template;
    [SerializeField] private Transform parents;
    private List<CardBaseUI> _listCardRendered;
    public void Start()
    {
        EventManager.Instance.RegisterListener(EventID.OnUpgradeCardSuccess, RerenderUI);
        
    }

    private void OnEnable()
    {
        RerenderUI(null);
    }

    private void RerenderUI(object obj)
    {
        if(_listCardRendered != null)
        {
            foreach (var item in _listCardRendered)
            {
                Destroy(item.gameObject);
            }
            
        }
        
        _listCardRendered = new List<CardBaseUI>();

        foreach (var card in _manager.GetMultiCard(_userModel.cardListID))
        {
            var cardUi = Instantiate(template, parents);
            cardUi.SetUI(card);
            _listCardRendered.Add(cardUi);
        }
    }

    public void FilterTowerCard(Toggle toggle)
    {
        if(!toggle.isOn)return;
        _listCardRendered.ForEach(card => card.gameObject.SetActive(card.CardData.TypeOfCard == CardType.TowerCard));
    }        
    public void FilterMonsterCard(Toggle toggle)
    {
        if(!toggle.isOn)return;
        _listCardRendered.ForEach(card => card.gameObject.SetActive(card.CardData.TypeOfCard == CardType.MonsterCard));
    }public void FilterSpellCard(Toggle toggle)
    {
        if(!toggle.isOn)return;
        _listCardRendered.ForEach(card => card.gameObject.SetActive(card.CardData.TypeOfCard == CardType.SpellCard));
    }    
    public void FilterAllCard(Toggle toggle)
    {
        if(!toggle.isOn)return;
        _listCardRendered.ForEach(card => card.gameObject.SetActive(true));
    }
}
