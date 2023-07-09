using System;
using System.Collections;
using System.Collections.Generic;
using MythicEmpire.Enums;
using MythicEmpire.InGame;
using MythicEmpire.Manager.MythicEmpire.Manager;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MonsterUI : MonoBehaviour
{
    public Sprite mySprite;
    public Sprite myBackgroundSprite;
    public Sprite rivalSprite;
    public Sprite rivalBackgroundSprite;

    public Image background;
    public Image fill;
    public TMPro.TMP_Text hpText;


    private void Update()
    {
        this.transform.LookAt(Camera.main.transform);
    }

    public void UpdateMonsterHp(int maxHp,int currentHp)
    {
        fill.fillAmount = (currentHp / maxHp);
        hpText.text = currentHp.ToString();
    }

    public void Init(int maxHp, bool isMyCastle)
    {
        if (isMyCastle)
        {
            background.sprite = myBackgroundSprite;
            fill.sprite = mySprite;
        }
        else
        {
            background.sprite = rivalBackgroundSprite;
            fill.sprite = rivalSprite;
        }

        fill.fillAmount = 1;
        hpText.text = maxHp.ToString();

    }
}
