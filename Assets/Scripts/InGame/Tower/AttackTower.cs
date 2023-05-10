using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MythicEmpire.InGame
{
    public class AttackTower : Tower
    {
        private int level;
        [SerializeField] private GameObject bullet;
        private bool canFire;

        // Start is called before the first frame update
        void Start()
        {
            stats = new TowerStats();
            stats.Damage = 4;
            stats.FireRange = 2;
            stats.ExploreRange = 0;
            stats.SpeedAttack = 1.5f;
            stats.BulletSpeed = 2;
            canFire = true;
        }

        // Update is called once per frame
        void Update()
        {
            Fire();
        }

        public void Fire()
        {
            if (canFire)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, stats.FireRange, InGameService.monsterLayerMask);

                if (colliders.Length > 0)
                {
                    GameObject target = null;
                    float minDistance = stats.FireRange;
                    foreach (Collider collider in colliders)
                    {
                        float distance = (transform.position - collider.transform.position).magnitude;
                        if (distance < minDistance)
                        {
                            Monster monsterComponent = collider.gameObject.GetComponent<Monster>();
                            if (isMyPlayer != monsterComponent.IsMyPlayer)
                            {
                                target = collider.gameObject;
                                minDistance = distance;
                            }
                        }
                    }
                    if (target != null)
                    {
                        transform.LookAt(target.transform.position);
                        GameObject b = Instantiate(bullet, transform.position, transform.rotation);
                        b.GetComponent<Bullet>().Init(target.transform, stats);
                        StartCoroutine(LoadBullet());
                    }
                }
            }
        }

        public void Upgrade()
        {

        }

        private IEnumerator LoadBullet()
        {
            canFire = false;
            yield return new WaitForSeconds(1 / stats.SpeedAttack);
            canFire = true;
        }
    }
}
