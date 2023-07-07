using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MythicEmpire.Card;
using static UnityEngine.GraphicsBuffer;

namespace MythicEmpire.InGame
{
    public class AttackTower : Tower
    {
        [SerializeField] private GameObject bullet;

        public override void Fire()
        {
            // if the tower can fire (by attack speed)
            if (canFire)
            {
                // get all monsters in range
                Collider[] colliders = Physics.OverlapSphere(transform.position, stats.FireRange, InGameService.monsterLayerMask);

                if (colliders.Length > 0)
                {
                    // get monster nearest tower
                    GameObject target = null;
                    float minDistance = stats.FireRange;
                    foreach (Collider collider in colliders)
                    {
                        var monsterComponent = collider.gameObject.GetComponent<Monster>();
                        if (!monsterComponent.IsDie)
                        {
                            float distance = (transform.position - collider.transform.position).magnitude;
                            if (distance < minDistance)
                            {
                                if (OwnerId != monsterComponent.OwnerId)
                                {
                                    target = collider.gameObject;
                                    minDistance = distance;
                                }
                            }
                        }
                    }
                    // fire to target
                    if (target != null)
                    {
                        // play fire animation
                        GetComponent<TowerAnimation>().PlayAnimation(id, "fire", target.transform);
                        // look at monster
                        transform.LookAt(target.transform.position);
                        // create bullet
                        GameObject b = Instantiate(bullet, transform.position, transform.rotation);
                        b.GetComponent<Bullet>().Init(target.transform, damage, exploreRange, bulletSpeed);
                        // wait to load bullet (by attack speed)
                        StartCoroutine(LoadBullet());
                        return;
                    }
                }
            }
            // play idle animation
            GetComponent<TowerAnimation>().PlayAnimation(id, "idle");
        }
    }
}
