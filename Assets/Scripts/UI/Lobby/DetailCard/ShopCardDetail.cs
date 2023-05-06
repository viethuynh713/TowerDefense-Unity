using System.Collections;
using System.Collections.Generic;
using MythicEmpire.Card;
using MythicEmpire.Enums;
using MythicEmpire.Manager;
using UnityEngine;

public class ShopCardDetail : MonoBehaviour
{
    [SerializeField] private MonterStatsRender _monterStatsRender;
    [SerializeField] private TowerStatsRender _towerStatsRender;
    [SerializeField] private SpellStatsRender _spellStatsRender;

    [SerializeField] private List<GameObject> _stars;

    [SerializeField] private TMPro.TMP_Text _nameTxt;

    private CardInfo _cardSelected;
    void Start()
    {
        this.gameObject.SetActive(false);
        EventManager.Instance.RegisterListener(EventID.SelectedCard,RenderCardSelected);
    }

    private void RenderCardSelected(object obj)
    {
        this.gameObject.SetActive(true);

        // var cardInfo = (CardInfo)obj;
        _cardSelected = (CardInfo)obj;
        _nameTxt.text = _cardSelected.CardName;
        SetStats(_cardSelected.CardStats);
        for (int i = 0; i < _stars.Count; i++)
        {
            _stars[i].SetActive(i < _cardSelected.CardStar);
        }
        
    }

    private void SetStats(StatsCard statsCard)
    {
        _monterStatsRender.gameObject.SetActive(false);
        _towerStatsRender.gameObject.SetActive(false);
        _spellStatsRender.gameObject.SetActive(false);
        if (statsCard.GetType() == typeof(TowerStats))
        {
            _towerStatsRender.gameObject.SetActive(true);
            _towerStatsRender.Render((TowerStats)statsCard);
        }        
        if (statsCard.GetType() == typeof(SpellStats))
        {
            _spellStatsRender.gameObject.SetActive(true);
            _spellStatsRender.Render((SpellStats)statsCard);
        }        
        if (statsCard.GetType() == typeof(MonsterStats))
        {
            _monterStatsRender.gameObject.SetActive(true);
            _monterStatsRender.Render((MonsterStats)statsCard);
        }
    }

    public void BuyCard()
    {
        Debug.Log(_cardSelected.CardName);
    }
    
}
