using UnityEngine;
using UnityEngine.Serialization;

namespace MythicEmpire.Card
{
    [CreateAssetMenu(fileName = "Card Data", menuName = "MythicEmpire Data/Data/Spell")]
    public class SpellStats : StatsCard
    {
        public float Time;
        public float Duration;
        public float DamegePerDuration;
        public float Range;
        public string Detail;
    }
}