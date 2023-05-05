using System.Collections.Generic;
using MythicEmpire.Card;
using MythicEmpire.Enums;
using MythicEmpire.Manager;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardBaseUI : MonoBehaviour
{
    [Header("Base Component")]
    [SerializeField] private Image frame;
    [SerializeField] private Image icon;
    [SerializeField] private Image avatar;
    [SerializeField] private List<Image> listStar;
    [SerializeField] private TMP_Text energyTxt;

    public CardInfo CardData;

    // public void Start()
    // {
    //      SetUI(CardData);
    //     Debug.Log(JsonConvert.SerializeObject(CardData));
    // }

    public void SetUI(CardInfo data)
    {
        this.CardData = data;
        SetIcon(data.TypeOfCard);
        SetFrame(data.CardRarity);
        SetStar(data.CardStar);
        avatar.sprite = CardData.CardImage;
        energyTxt.text = data.CardStats.Energy.ToString();

    }

    private void SetStar(int star)
    {
        listStar.ForEach(image => image.gameObject.SetActive(false));

        for (int i = 0; i < star; i++)
        {
            listStar[i].gameObject.SetActive(true);
        }
    }

    private void SetFrame(RarityCard rarity)
    {
        switch (rarity)
        {
            case RarityCard.Common:
                frame.sprite = (Sprite)Resources.LoadAsync<Sprite>("common-frame").asset;
                break;
            case RarityCard.Rare:
                frame.sprite = (Sprite)Resources.LoadAsync<Sprite>("rare-frame").asset;
                break;
            case RarityCard.Mythic:                
                frame.sprite = (Sprite)Resources.LoadAsync<Sprite>("mythic-frame").asset;
                break;
            case RarityCard.Legend:
                frame.sprite = (Sprite)Resources.LoadAsync<Sprite>("legend-frame").asset;
                break;
        }
    }

    private void SetIcon(CardType type)
    {
        switch (type)
        {
            case CardType.MonsterCard:
                icon.sprite = (Sprite)Resources.LoadAsync<Sprite>("monster").asset;
                break;            
            case CardType.TowerCard:
                icon.sprite = (Sprite)Resources.LoadAsync<Sprite>("tower").asset;
                break;            
            case CardType.SpellCard:
                icon.sprite = (Sprite)Resources.LoadAsync<Sprite>("mana").asset;
                break;
        }
    }

    public void SelectedButtonClick()
    {
        EventManager.Instance.PostEvent(EventID.SelectedCard,CardData);
    }
}
