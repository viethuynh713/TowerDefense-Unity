using System;
using System.Collections;
using System.Collections.Generic;
using MythicEmpire.Enums;
using MythicEmpire.InGame;
using MythicEmpire.Manager.MythicEmpire.Manager;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CastleHpUI : MonoBehaviour
{
    
    public Sprite mySprite;
    public Sprite myBackgroundSprite;
    public Sprite rivalSprite;
    public Sprite rivalBackgroundSprite;

    public Image background;
    public Image fill;
    public TMPro.TMP_Text hpText;

    
    private string playerId;

    private void Start()
    {
        EventManager.Instance.RegisterListener(EventID.UpdateCastleHp,(o)=>GameController_v2.Instance.mainThreadAction.Add(()=>UpdateCastleHp(o)));
    }

    private void UpdateCastleHp(object data)
    {
        var castleData = (JObject)data;
        var id = castleData["userid"].ToString();

        var newHp = (int)castleData["newCastleHp"];

        if (playerId != id) return;

        fill.fillAmount = (newHp / 20);
        hpText.text = newHp.ToString();
    }

    public void Init(string id, bool isMyCastle)
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

        playerId = id;
        hpText.text = "20";
    }
}
