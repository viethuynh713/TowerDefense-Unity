using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MythicEmpire.InGame
{
    public class DirectionalBullet : Bullet
    {
        
        public override void Move()
        {
            // if target is die, explore bullet
            if (target == null || target.IsDie)
            {
                Destroy(gameObject);

            }
            // otherwise move to target and explore if colliding target
            else
            {
                transform.LookAt(target.transform);
                
                transform.position = Vector3.MoveTowards(transform.position, 
                    new Vector3(target.transform.position.x, transform.position.y,target.transform.position.z), 
                    bulletSpeed * Time.deltaTime);
                
                if (Vector3.Distance(transform.position,
                        new Vector3(transform.position.x, transform.position.y,target.transform.position.z)) < InGameService.infinitesimal)
                {
                    Explore();
                }
            }
        }

        public override void Explore()
        {
            if (target != null && !target.IsDie)
            {
                target.TakeDamage(damage);
            }
            Destroy(gameObject);
        }

    }
}
