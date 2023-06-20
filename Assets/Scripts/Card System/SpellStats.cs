using UnityEngine;

namespace MythicEmpire.Card
{
    [CreateAssetMenu(fileName = "Card Data", menuName = "MythicEmpire Data/Data/Spell")]
    public class SpellStats : StatsCard
    {
        public float Time;
        public float Duration;
        public float Value;
        public float Range;
        public string Detail;
    }
}