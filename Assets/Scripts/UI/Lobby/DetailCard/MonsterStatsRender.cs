using System.Collections;
using System.Collections.Generic;
using MythicEmpire.Card;
using TMPro;
using UnityEngine;

namespace MythicEmpire.UI.Lobby
{
    public class MonsterStatsRender : MonoBehaviour
    {
        [SerializeField] private TMP_Text hpText;
        [SerializeField] private TMP_Text damageText;
        [SerializeField] private TMP_Text moveSpeedText;
        [SerializeField] private TMP_Text attackSpeedText;
        [SerializeField] private TMP_Text energyText;

        public void Render(MonsterStats stats)
        {
            hpText.text = stats.Hp.ToString();
            damageText.text = stats.Damage.ToString();
            moveSpeedText.text = stats.MoveSpeed.ToString();
            attackSpeedText.text = stats.AttackSpeed.ToString();
            energyText.text = stats.Energy.ToString();
        }
    }
}
