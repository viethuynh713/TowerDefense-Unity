using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MythicEmpire.InGame
{
    public class ZoneBullet : Bullet
    {
        [SerializeField]private ParticleSystem exploreVfx;
        public override void Move()
        {
            // if target is die, explore bullet
            if (target == null)
            {
                Explore();
            }
            // otherwise move to target and explore if colliding target
            else
            {
                transform.LookAt(target.transform.position);
                
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
            Collider[] results = new Collider[20];
            var numColliders = Physics.OverlapSphereNonAlloc(transform.position, exploreRange, results);

            for (int i = 0; i < numColliders; i++)
            {
                
                if (results[i].TryGetComponent <Monster>(out var monster))
                {
                    monster.TakeDamage(damage);
                }
            }
            Destroy(gameObject);

        }

        private void OnDestroy()
        {
            Instantiate(exploreVfx, transform.position, transform.rotation);
        }
    }
}
