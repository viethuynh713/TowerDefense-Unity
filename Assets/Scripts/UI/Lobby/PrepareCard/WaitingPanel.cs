using System;
using System.Collections;
using System.Collections.Generic;
using MythicEmpire.Enums;
using MythicEmpire.Manager.MythicEmpire.Manager;
using TMPro;
using UnityEngine;

public class WaitingPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _timeText;
    void Start()
    {
        _timeText.text = "00:00";

        EventManager.Instance.RegisterListener(EventID.ServerReceiveMatchMaking,DisplayTime);
    }

    private void DisplayTime(object time)
    {
        TimeSpan timeSpan = TimeSpan.FromMinutes((int)time);
        string formattedTime = timeSpan.ToString(@"hh\:mm");
        Debug.Log(formattedTime);
        _timeText.text = formattedTime;
    }
    
}
