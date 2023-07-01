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
            EventManager.Instance.RegisterListener(EventID.RenderListCard, (o)=>GameController_v2.Instance.mainThreadAction.Add(()=>RenderListCard(o)));
            EventManager.Instance.RegisterListener(EventID.OnEndGame, HandleEndGame);

        }

        private void HandleEndGame(object result)
        {
            _endGameUI.gameObject.SetActive(true);
            _endGameUI.ShowResult((GameResult)result, 145);
        }

        private void RenderListCard(object listCard)
        {
            var listMyCard = (List<string>)listCard;
            Debug.Log("Count card: "+listMyCard.Count);
            foreach (var cardId in listMyCard)
            {
                Debug.Log("card id: "+cardId);

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