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
        EventManager.Instance.RegisterListener(EventID.SelectedCard, RenderSelectedCard);
        listItems = new List<CardBaseUI>();
        foreach (var card in _manager.GetMultiCard(RarityCard.Common))
        {
            var cardUi = Instantiate(template, parents);
            cardUi.SetUI(card);
            listItems.Add(cardUi);
        }
        foreach (var card in _manager.GetMultiCard(RarityCard.Rare))
        {
            var cardUi = Instantiate(template, parents);
            cardUi.SetUI(card);
            listItems.Add(cardUi);

        }
        
        // string json = "{\"CardId\":\"d7fb68f1-9adb-4127-b5a7-6432ecefd319\",\"CardName\":\"Diamond Tower\",\"CardStar\":0,\"TypeOfCard\":1,\"CardRarity\":2,\"CardStats\":{\"Damage\":6 ,\"Range\":1.0,\"AttackSpeed\":1.0,\"Energy\":2,\"name\":\"d7fb68f1-9adb-4127-b5a7-6432ecefd319\",\"hideFlags\":0},\"name\":\"d7fb68f1-9adb-4127-b5a7-6432ecefd319\",\"hideFlags\":0}";
        // var obj = JObject.Parse(json);
        //
        // StatsCard c = JsonConvert.DeserializeObject<TowerStats>((obj["CardStats"]).ToString());
        // Debug.Log(((TowerStats)c).Damage);
    }

    private void RenderSelectedCard(object obj)
    {
        Debug.Log(((CardInfo)obj).CardName);
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
