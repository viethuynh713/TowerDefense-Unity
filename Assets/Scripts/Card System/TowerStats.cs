using UnityEngine;

namespace MythicEmpire.Card
{
    [CreateAssetMenu(fileName = "Card Data", menuName = "MythicEmpire Data/Data/Tower")]
    public class TowerStats : StatsCard
    {
        public int Damage;
        public float AttackSpeed;
        public float FireRange;
        public float ExploreRange;
        public float BulletSpeed;
    }
}
