using System;
using System.Collections;
using System.Collections.Generic;
using MythicEmpire.Card;
using MythicEmpire.Enums;
using MythicEmpire.Manager.MythicEmpire.Manager;
using MythicEmpire.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace MythicEmpire.InGame.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text _energyTxt;
        [SerializeField]private CardBaseUI _cardUIPrefab;
        [SerializeField] private Transform _parentCards;
        [SerializeField] private Slider _waveTimeSlider;
        [SerializeField] private Slider _energySlider;

        [SerializeField] private EndGameUI _endGameUI;
        [Inject] private CardManager _cardManager;
        public void Start()
        {
            _endGameUI.gameObject.SetActive(false);
            EventManager.Instance.RegisterListener(EventID.UpdateEnergy,UpdateEnergy);
            EventManager.Instance.RegisterListener(EventID.UpdateWaveTime, UpdateWaveTime);
            EventManager.Instance.RegisterListener(EventID.RenderListCard, RenderListCard);
            EventManager.Instance.RegisterListener(EventID.OnEndGame, HandleEndGame);
            List<string> cards = new List<string>()
            {
                "02dc58b9-11b2-4094-aac7-a8f21b114790",
                "acb777a0-4abc-45a9-90c7-15ea01ed3c7b",
                "2b770545-adaf-4cc6-b5dc-60ada0a3c49c",
                "38842417-c8ae-47d7-afdf-af5afd820677",
                "01ab28d7-8362-4747-82db-df118b3bcd47",
                "d4102b9f-4ccc-4754-8303-b78453bbf166",
                "2bf570ee-a62d-4d59-9db4-1e97d519e7ce"
            };
            RenderListCard(cards);
            
            StartCoroutine(TestEndGame());
        }

        private IEnumerator TestEndGame()
        {
            yield return new WaitForSeconds(2);
            UpdateEnergy(100);
            yield return new WaitForSeconds(2);
            UpdateEnergy(70);
            yield return new WaitForSeconds(2);
            UpdateEnergy(60);
            yield return new WaitForSeconds(2);
            UpdateEnergy(50);
            yield return new WaitForSeconds(2);
            UpdateEnergy(0);
            yield return new WaitForSeconds(2);
            UpdateEnergy(20);
            yield return new WaitForSeconds(2);
            UpdateEnergy(4);
            yield return new WaitForSeconds(2);
            UpdateEnergy(56);
            yield return new WaitForSeconds(2);
            UpdateEnergy(69);
            yield return new WaitForSeconds(2);
            UpdateEnergy(100);
        
        
            
        }
        private void HandleEndGame(object result)
        {
            _endGameUI.gameObject.SetActive(true);
            _endGameUI.ShowResult((GameResult)result, 145);
        }

        private void RenderListCard(object listCard)
        {
            var listMyCard = (List<string>)listCard;
            foreach (var cardId in listMyCard)
            {
                var cardUI = Instantiate(_cardUIPrefab, _parentCards);
                cardUI.SetUI(_cardManager.GetCardById(cardId));
            }
        }

        private void UpdateWaveTime(object obj)
        {
            
        }

        private void UpdateEnergy(object newEnergy)
        {
            _energySlider.value = _energySlider.maxValue - (int)newEnergy;
            _energyTxt.text = newEnergy.ToString();
        }
    }
}