using System.Collections.Generic;
using MythicEmpire.Card;
using MythicEmpire.Enums;
using MythicEmpire.Manager;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class ShopCardUI : MonoBehaviour
{
    [Inject] private CardManager _manager;
    [SerializeField] private CardBaseUI template;
    [SerializeField] private Transform parents;
    private List<CardBaseUI> listItems;
    public void Start()
    {
  
        listItems = new List<CardBaseUI>();
        foreach (var card in _manager.GetMultiCard())
        {
            var cardUi = Instantiate(template, parents);
            cardUi.SetUI(card);
            listItems.Add(cardUi);
        }
        // foreach (var card in _manager.GetMultiCard(RarityCard.Common))
        // {
        //     var cardUi = Instantiate(template, parents);
        //     cardUi.SetUI(card);
        //     listItems.Add(cardUi);
        // }
        // foreach (var card in _manager.GetMultiCard(RarityCard.Rare))
        // {
        //     var cardUi = Instantiate(template, parents);
        //     cardUi.SetUI(card);
        //     listItems.Add(cardUi);
        //
        // }
    }
    
    public void FilterTower(Toggle toggle)
    {
        if(!toggle.isOn)return;
        listItems.ForEach(card => card.gameObject.SetActive(card.CardData.TypeOfCard == CardType.TowerCard));
    }        
    public void FilterMonster(Toggle toggle)
    {
        if(!toggle.isOn)return;
        listItems.ForEach(card => card.gameObject.SetActive(card.CardData.TypeOfCard == CardType.MonsterCard));
    }public void FilterSupport(Toggle toggle)
    {
        if(!toggle.isOn)return;
        listItems.ForEach(card => card.gameObject.SetActive(card.CardData.TypeOfCard == CardType.SpellCard));
    }    
    public void FilterAll(Toggle toggle)
    {
        if(!toggle.isOn)return;
        listItems.ForEach(card => card.gameObject.SetActive(true));
    }
}
