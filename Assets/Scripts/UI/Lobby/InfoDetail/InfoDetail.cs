using System;
using System.Collections;
using System.Collections.Generic;
using MythicEmpire.Card;
using MythicEmpire.Networking;
using MythicEmpire.Model;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

namespace MythicEmpire.UI.Lobby
{


    public class InfoDetail : MonoBehaviour
    {
        [Inject] private CardManager _cardManager;
        [Inject] private UserModel _userModel;
        [Inject] private IUserInfosNetwork _userInfosNetwork;

        [SerializeField] private ShowUserStatsCard _showUserStatsCard;
        [SerializeField] private TMP_InputField _nicknameText;
        [SerializeField] private TMP_Text _idText;
        [SerializeField] private TMP_Text _rankText;
        [SerializeField] private TMP_Text _emailText;

        private void Start()
        {
            _nicknameText.onEndEdit.AddListener(OnEndEditNickName);
        }

        private void OnEnable()
        {
            ShowInfoDetail();

        }

        private void ShowInfoDetail()
        {
            _showUserStatsCard.ShowStatCard(_cardManager.GetMultiCard(_userModel.cardListID));
            _idText.text = $"PlayerId: {_userModel.userId}";
            _nicknameText.text = _userModel.nickName;
            _rankText.text = $"Rank: {_userModel.rank.ToString()}";
            _emailText.text = $"Email: {_userModel.email}";
            
        }

        public void OnEndEditNickName(string newName)
        {
            Debug.Log(newName);
        }

        public void LogoutButtonClick()
        {
            SceneManager.LoadSceneAsync("Menu");
        }
    }
}
