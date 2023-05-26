using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MythicEmpire
{
    public class MonsterStats : StatsCard
    {
        private int hp;
        private float attackSpeed;
        private float moveSpeed;
        private float attackRange;
        private int damage;

        public int Hp { get { return hp; } set { hp = value; } }
        public float AttackSpeed { get { return attackSpeed; } set { attackSpeed = value; } }
        public float MoveSpeed { get { return moveSpeed; } set {  moveSpeed = value; } }
        public float AttackRange { get { return attackRange; } set {  attackRange = value; } }
        public int Damage { get { return damage; } set { damage = value; } }
    }
}