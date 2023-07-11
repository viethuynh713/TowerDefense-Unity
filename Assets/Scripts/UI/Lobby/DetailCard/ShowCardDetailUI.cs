using System;
using System.Collections;
using System.Collections.Generic;
using MythicEmpire.Card;
using MythicEmpire.Enums;
using MythicEmpire.Manager;
using MythicEmpire.Manager.MythicEmpire.Manager;
using MythicEmpire.Networking;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VContainer;
namespace MythicEmpire.UI.Lobby
{
    public class ShowCardDetailUI : MonoBehaviour
    {
        [SerializeField] private MonsterStatsRender _monsterStatsRender;
        [SerializeField] private TowerStatsRender _towerStatsRender;
        [SerializeField] private SpellStatsRender _spellStatsRender;
        [SerializeField] private Image _mainImage;

        [SerializeField] private List<GameObject> _stars;

        [SerializeField] private TMPro.TMP_Text _nameTxt;
        [SerializeField] private Button button;

        [Inject] private ICardServiceNetwork _cardServiceNetwork;
        [Inject] private CardManager _cardManager;
        private CardInfo _cardSelected;

        private void SetActivityInfo(bool activity)
        {
            _monsterStatsRender.gameObject.SetActive(activity);
            _towerStatsRender.gameObject.SetActive(activity);
            _spellStatsRender.gameObject.SetActive(activity);
            _mainImage.gameObject.SetActive(activity);
            for (int i = 0; i < _stars.Count; i++)
            {
                _stars[i].SetActive(activity);
            }

            _nameTxt.gameObject.SetActive(activity);
            button.gameObject.SetActive(activity);

        }

        void Start()
        {
            EventManager.Instance.RegisterListener(EventID.SelectedCard, RenderCardSelected);
            EventManager.Instance.RegisterListener(EventID.OnUpgradeCardSuccess, RenderCardSelected);

        }

        private void OnEnable()
        {
            SetActivityInfo(false);
        }

        private void RenderCardSelected(object id)
        {
            SetActivityInfo(true);
            _cardSelected = _cardManager.GetCardById((string)id);
            _nameTxt.text = _cardSelected.CardName;
            InstanceObject(_cardSelected.GameObjectPrefab);
            SetStats(_cardSelected.CardStats);
            for (int i = 0; i < _stars.Count; i++)
            {
                _stars[i].SetActive(i < _cardSelected.CardStar);
            }

        }

        private void InstanceObject(GameObject gameObject)
        {

            Destroy(_mainImage.transform.GetChild(0).gameObject);


            if (gameObject == null) return;
            var obj = Instantiate(gameObject, _mainImage.transform);
            obj.transform.localPosition = Vector3.zero;
        }

        private void SetStats(StatsCard statsCard)
        {
            _monsterStatsRender.gameObject.SetActive(false);
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
                _monsterStatsRender.gameObject.SetActive(true);
                _monsterStatsRender.Render((MonsterStats)statsCard);
            }
            // Debug.Log(statsCard.GetType());
        }

        public void BuyCardButtonClick()
        {

            _cardServiceNetwork.PurchaseCardRequest(_cardSelected.CardId);
        }

        public void UpgradeCardButtonClick()
        {

            _cardServiceNetwork.UpgradeCardRequest(_cardSelected.CardId);


        }
    }
}
