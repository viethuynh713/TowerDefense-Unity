using System;
using System.Collections;
using System.Collections.Generic;
using MythicEmpire.Card;
using MythicEmpire.Enums;
using MythicEmpire.Manager.MythicEmpire.Manager;
using MythicEmpire.Model;
using MythicEmpire.UI;
using Networking_System.Model.ReceiveData;
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
        [Inject] private UserModel _userModel;
        public void Start()
        {
            _endGameUI.gameObject.SetActive(false);
            EventManager.Instance.RegisterListener(EventID.UpdateEnergy,(o)=>GameController_v2.Instance.mainThreadAction.Add(()=>UpdateEnergy(o)));
            EventManager.Instance.RegisterListener(EventID.UpdateWaveTime, (o)=>GameController_v2.Instance.mainThreadAction.Add(()=>UpdateWaveTime(o)));
            EventManager.Instance.RegisterListener(EventID.RenderListCard, (o)=>GameController_v2.Instance.mainThreadAction.Add(()=>RenderListCard(o)));
            EventManager.Instance.RegisterListener(EventID.OnEndGame,(o)=>GameController_v2.Instance.mainThreadAction.Add(()=>HandleEndGame(o)));

        }

        private void HandleEndGame(object result)
        {
            _endGameUI.gameObject.SetActive(true);
            var data = (EndGameDataSender)result;
            if (data.playerLose == _userModel.userId)
            {
                _endGameUI.ShowResult(GameResult.Loss, data.totalTime);
            }
            else
            {
                _endGameUI.ShowResult(GameResult.Win, data.totalTime);

            }
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
            var wave = (Wave)obj;
            // Debug.Log($"WaveTime: {wave.maxTimeWaiting}/{wave.currentTime}");
            _waveTimeSlider.maxValue = wave.maxTimeWaiting;
            _waveTimeSlider.value = wave.currentTime;
        }

        private void UpdateEnergy(object newEnergy)
        {
            // Debug.Log("set energy:"+newEnergy.ToString());
            _energySlider.value = _energySlider.maxValue - (int)newEnergy;
            _energyTxt.text = newEnergy.ToString();
        }
    }
}