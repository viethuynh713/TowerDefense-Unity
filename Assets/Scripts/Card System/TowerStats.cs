using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MythicEmpire
{
    public class TowerStats : StatsCard
    {
        private int damage;
        private int fireRange;
        private float exploreRange;
        private float speedAttack;
        private float bulletSpeed;

        public int Damage { get { return damage; } set { damage = value; } }
        public int FireRange { get { return fireRange; } set { fireRange = value; } }

        public float ExploreRange { get { return exploreRange; } set { exploreRange = value; } }
        public float SpeedAttack { get { return speedAttack; } set { speedAttack = value; } }
        public float BulletSpeed { get { return bulletSpeed; } set { bulletSpeed = value; } }
    }
}
