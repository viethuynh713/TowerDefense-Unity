using System.Collections;
using System.Collections.Generic;
using MythicEmpire.Card;
using TMPro;
using UnityEngine;

public class SpellStatsRender : MonoBehaviour
{
    [SerializeField] private TMP_Text durationText;
    [SerializeField] private TMP_Text rangeText;
    [SerializeField] private TMP_Text energyText;
    [SerializeField] private TMP_Text detailText;

    public void Render(SpellStats stats)
    {

        durationText.text = stats.Duration.ToString();
        rangeText.text = stats.Range.ToString();
        energyText.text = stats.Energy.ToString();
        detailText.text = stats.Detail;
    }
}
