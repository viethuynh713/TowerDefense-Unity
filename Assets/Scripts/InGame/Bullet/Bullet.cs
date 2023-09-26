using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MythicEmpire.Card;

namespace MythicEmpire.InGame
{
    public class Bullet : MonoBehaviour
    {
        protected int id;
        protected string ownerId;
        protected Monster target;

        protected int damage;
        protected float exploreRange;
        protected float bulletSpeed;

        public void Init(Monster target,string owner, int damage, float exploreRange, float bulletSpeed)
        {
            this.target = target;
            this.damage = damage;
            this.exploreRange = exploreRange;
            this.bulletSpeed = bulletSpeed;
            this.ownerId = owner;
        }

        // Update is called once per frame
        void Update()
        {
            Move();
        }

        public virtual void Move()
        {

        }

        public virtual void Explore()
        {

        }
    }
}
