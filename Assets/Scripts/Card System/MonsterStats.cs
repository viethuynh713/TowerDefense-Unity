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
        private float detectRange;
        private float attackRange;
        private int damage;

        public float MoveSpeed { get { return moveSpeed; } set {  moveSpeed = value; } }
        public float DetectRange { get {  return detectRange; } set {  detectRange = value; } }
        public float AttackRange { get { return attackRange; } set {  attackRange = value; } }
    }
}
