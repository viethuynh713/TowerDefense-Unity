using System.Collections.Generic;
using MythicEmpire.Card;
using MythicEmpire.Enums;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class ShopCardUI : MonoBehaviour
{
    [Inject] private CardManager _cardManager;
    [SerializeField] private CardBaseUI template;
    [SerializeField] private Transform parents;
    private List<CardBaseUI> _listCardRendered;
    public void Start()
    {
        _listCardRendered = new List<CardBaseUI>();
        foreach (var card in _cardManager.GetMultiCard(RarityCard.Common))
        {
            var cardUi = Instantiate(template, parents);
            cardUi.SetUI(card);
            _listCardRendered.Add(cardUi);
        }
        foreach (var card in _cardManager.GetMultiCard(RarityCard.Rare))
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
    }
    public void FilterSpellCard(Toggle toggle)
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
