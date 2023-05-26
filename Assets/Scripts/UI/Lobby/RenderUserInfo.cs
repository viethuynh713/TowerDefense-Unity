using System;
using System.Collections;
using System.Collections.Generic;
using MythicEmpire.Enums;
using MythicEmpire.Manager;
using MythicEmpire.Manager.MythicEmpire.Manager;
using MythicEmpire.Model;
using TMPro;
using UnityEngine;
using VContainer;

namespace MythicEmpire.UI.Lobby
{
    public class RenderUserInfo : MonoBehaviour
    {
        [Inject] private UserModel _userModel;

        [SerializeField] private TMP_Text userName;
        [SerializeField] private TMP_Text rank;
        [SerializeField] private TMP_Text gold;

        void Start()
        {
            EventManager.Instance.RegisterListener(EventID.OnUpdateNicknameSuccess,
                o => userName.text = _userModel.nickName);
            EventManager.Instance.RegisterListener(EventID.OnUpgradeCardSuccess, UpdateGoldText);
            EventManager.Instance.RegisterListener(EventID.OnBuyCardSuccess, UpdateGoldText);
            EventManager.Instance.RegisterListener(EventID.OnBuyGachaSuccess, UpdateGoldText);
            gold.text = _userModel.gold.ToString();
            rank.text = String.Concat("Rank: ", _userModel.rank.ToString());
            userName.text = _userModel.nickName;
            Notification.Instance.NotifyStatus($"Welcome {_userModel.nickName} comeback");
        }

        private void UpdateGoldText(object o)
        {
            gold.text = _userModel.gold.ToString();
        }

    }
}
