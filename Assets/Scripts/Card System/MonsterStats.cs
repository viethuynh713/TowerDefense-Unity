using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MythicEmpire.Card
{
    [CreateAssetMenu(fileName = "Card Data", menuName = "MythicEmpire Data/Data/Monster")]
    public class MonsterStats : StatsCard
    {
        public int Hp;
        public float AttackSpeed;
        public float MoveSpeed;
        public float AttackRange;
        public int Damage;
        public int EnergyGainWhenDie;
    }
}