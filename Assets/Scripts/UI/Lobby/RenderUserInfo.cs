using System;
using System.Collections;
using System.Collections.Generic;
using MythicEmpire.Enums;
using MythicEmpire.Manager;
using MythicEmpire.PlayerInfos;
using TMPro;
using UnityEngine;
using VContainer;

public class RenderUserInfo : MonoBehaviour
{
    [Inject] private UserModel _userModel;

    [SerializeField] private TMP_Text userName;
    [SerializeField] private TMP_Text rank;
    [SerializeField] private TMP_Text gold;
    void Start()
    {
        EventManager.Instance.RegisterListener(EventID.OnUpdateNicknameSuccess, o => userName.text = _userModel.nickName);
        EventManager.Instance.RegisterListener(EventID.OnUpgradeCardSuccess, o => gold.text = _userModel.gold.ToString());
        EventManager.Instance.RegisterListener(EventID.OnBuyCardSuccess, o => gold.text = _userModel.gold.ToString());
        gold.text = _userModel.gold.ToString();
        rank.text = String.Concat("Rank: ",_userModel.rank.ToString());
        userName.text = _userModel.nickName;
    }


}
