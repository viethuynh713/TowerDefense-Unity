using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MythicEmpire.Card;
using static UnityEngine.GraphicsBuffer;

namespace MythicEmpire.InGame
{
    public class AttackTower : Tower
    {
        [SerializeField] protected Bullet bullet;
        [SerializeField] protected Transform firePosition;
        protected Monster Target = null;
        
        protected override void Fire()
        {

            if (Target != null)
            {
                transform.LookAt(new Vector3(Target.transform.position.x, transform.position.y,Target.transform.position.z));
                CheckTargetRange();
            }

            if (canFire)
            {

                    if (Target == null || Target.IsDie)
                    {
                        Target = null;
                        FindMonsterTarget();
                    }
                    if (Target != null)
                    {
                        transform.LookAt(new Vector3(Target.transform.position.x, transform.position.y,Target.transform.position.z));

                        animation.PlayAnimation("fire");
                        // create bullet
                        Bullet b = Instantiate(bullet, firePosition.position, firePosition.rotation);
                        b.Init(Target, _ownerId,damage, exploreRange, bulletSpeed);
                        
                        StartCoroutine(LoadBullet());
                        CheckTargetRange();

                    }

            } 
        }
        

        protected void FindMonsterTarget()
        {
            // get all monsters in range
            Collider[] colliders = Physics.OverlapSphere(transform.position, fireRange, InGameService.monsterLayerMask);

            if (colliders.Length > 0)
            {

                // float minDistance = stats.FireRange;
                foreach (Collider collider in colliders)
                {
                    if (!collider.gameObject.TryGetComponent<Monster>(out var monsterComponent)) return;
                    if (monsterComponent.OwnerId == _ownerId) return;
                    if (!monsterComponent.IsDie)
                    {
                        Target = monsterComponent;
                        
                    }
                }

            }
        }

        protected void CheckTargetRange()
        {
            if (Vector3.Distance(transform.position, new Vector3(Target.transform.position.x, transform.position.y,Target.transform.position.z)) > fireRange)
            {
                animation.PlayAnimation("stop-fire");
                Target = null;
            }
        }
    }
}
