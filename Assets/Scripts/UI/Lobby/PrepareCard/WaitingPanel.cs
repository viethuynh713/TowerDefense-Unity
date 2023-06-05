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
        EventManager.Instance.RegisterListener(EventID.ServeReceiveMatchMaking,DisplayTime);
    }

    private void DisplayTime(object obj)
    {
        _timeText.text = (string)obj;
    }
    
}
