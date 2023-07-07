using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MythicEmpire.InGame
{
    public class ZoneBullet : Bullet
    {
        public override void Move()
        {
            // if target is die, explore bullet
            if (target == null)
            {
                DealDamage(damage);
            }
            // otherwise move to target and explore if colliding target
            else
            {
                transform.LookAt(target.position);
                transform.position = Vector3.MoveTowards(transform.position, target.position, bulletSpeed * Time.deltaTime);
                if ((target.position - transform.position).magnitude < InGameService.infinitesimal)
                {
                    DealDamage(damage);
                }
            }
        }

        private void DealDamage(int damage)
        {
            // if bullet is explore, deal dmg in a zone
            if (exploreRange > 0)
            {
                Monster[] monsterList = FindObjectsOfType<Monster>();
                foreach (Monster monster in monsterList)
                {
                    if ((monster.transform.position - transform.position).magnitude < exploreRange)
                    {
                        monster.TakeDamage(damage);
                    }
                }
            }
            // otherwise if target is still alive, deal dmg to target
            else if (target != null)
            {
                target.gameObject.GetComponent<Monster>().TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
