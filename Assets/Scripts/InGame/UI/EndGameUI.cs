using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MythicEmpire.Enums;
using TMPro;
using UnityEngine.SceneManagement;

public class EndGameUI : MonoBehaviour
{
    [SerializeField] private TMP_Text resultTxt;
    [SerializeField] private TMP_Text totalTimeTxt;
    public void ShowResult(GameResult result, int totalTime)
    {
        if (result == GameResult.Loss)
        {
            resultTxt.text = "LOSE";
            resultTxt.color = new Color(0.7f, 0.7f, 0.7f);
            
            TimeSpan timeSpan = TimeSpan.FromMinutes(totalTime);
            string timeString = timeSpan.ToString(@"hh\:mm");
            totalTimeTxt.text = $"Total time: {timeString}";

        }
        if (result == GameResult.Win)
        {
            resultTxt.text = "WIN";
            resultTxt.color = new Color(0.95f, 0.9f, 0.4f);
            
            TimeSpan timeSpan = TimeSpan.FromMinutes(totalTime);
            string timeString = timeSpan.ToString(@"hh\:mm");
            totalTimeTxt.text = $"Total time: {timeString}";
            
        }
    }

    public void GoToLobby()
    {
        SceneManager.LoadSceneAsync("Lobby");
    }
}
