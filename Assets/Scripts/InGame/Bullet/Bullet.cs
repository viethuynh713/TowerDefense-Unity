using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MythicEmpire.Card;

namespace MythicEmpire.InGame
{
    public class Bullet : MonoBehaviour
    {
        protected int id;
        protected int ownerId;
        protected Transform target;
        protected TowerStats stats;
        // Start is called before the first frame update
        void Start()
        {

        }

        public void Init(Transform target, TowerStats stats)
        {
            this.target = target;
            this.stats = stats;
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
