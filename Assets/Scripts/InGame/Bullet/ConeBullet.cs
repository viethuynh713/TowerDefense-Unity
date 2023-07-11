using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MythicEmpire.InGame
{
    public class ConeBullet : Bullet
    {
        public override void Move()
        {
            DealDamage(damage);
        }

        private void DealDamage(int damage)
        {
            Monster[] monsterList = FindObjectsOfType<Monster>();
            foreach (Monster monster in monsterList)
            {
                // check if monster is in explore range and in degree of explore
                if ((monster.transform.position - transform.position).magnitude < exploreRange)
                {
                    if (Vector3.Angle(monster.transform.position - transform.position, target.position - transform.position) <= 60)
                    {
                        monster.TakeDamage(damage);
                    }
                }
            }
            Destroy(gameObject);
        }
    }
}
