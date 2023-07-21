using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MythicEmpire.Card;
using static UnityEngine.GraphicsBuffer;

namespace MythicEmpire.InGame
{
    public class AttackTower : Tower
    {
        [SerializeField] private Bullet bullet;
        [SerializeField] private Transform firePosition;
        private Monster _target = null;
        
        protected override void Fire()
        {

            if (_target != null)
            {
                transform.LookAt(_target.transform.position);
                CheckTargetRange();
            }

            if (canFire)
            {

                    if (_target == null || _target.IsDie)
                    {
                        _target = null;
                        FindMonsterTarget();
                    }
                    if (_target != null)
                    {
                        transform.LookAt(_target.transform.position);

                        animation.PlayAnimation("fire");
                        // create bullet
                        Bullet b = Instantiate(bullet, firePosition.position, firePosition.rotation);
                        b.Init(_target, damage, exploreRange, bulletSpeed);
                        
                        StartCoroutine(LoadBullet());
                        CheckTargetRange();

                    }

            } 
        }
        

        private void FindMonsterTarget()
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
                        _target = monsterComponent;
                        
                    }
                }

            }
        }

        private void CheckTargetRange()
        {
            if (Vector3.Distance(transform.position, new Vector3(_target.transform.position.x, transform.position.y,_target.transform.position.z)) > fireRange)
            {
                _target = null;
            }
        }
    }
}
