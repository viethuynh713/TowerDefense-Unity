using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MythicEmpire.InGame
{
    public class DirectionalBullet : Bullet
    {
        public override void Move()
        {
            if (target == null)
            {
                Destroy(gameObject);
            }
            else
            {
                transform.LookAt(target.position);
                transform.position = Vector3.MoveTowards(transform.position, target.position, stats.BulletSpeed * Time.deltaTime);
                if ((target.position - transform.position).magnitude < InGameService.infinitesimal)
                {
                    target.gameObject.GetComponent<Monster>().TakeDmg(stats.Damage);
                    Destroy(gameObject);
                }
            }
        }
    }
}
