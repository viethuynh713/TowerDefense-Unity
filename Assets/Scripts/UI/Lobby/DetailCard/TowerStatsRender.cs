
using MythicEmpire.Card;
using UnityEngine;
using TMPro;

namespace MythicEmpire.UI.Lobby
{
    public class TowerStatsRender : MonoBehaviour
    {
        [SerializeField] private TMP_Text damageText;
        [SerializeField] private TMP_Text rangeText;
        [SerializeField] private TMP_Text attackSpeedText;
        [SerializeField] private TMP_Text energyText;

        public void Render(TowerStats stats)
        {

            damageText.text = stats.Damage.ToString();
            attackSpeedText.text = stats.AttackSpeed.ToString();
            rangeText.text = stats.FireRange.ToString();
            energyText.text = stats.Energy.ToString();
        }
    }
}
