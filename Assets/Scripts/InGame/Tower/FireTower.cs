using UnityEngine;

namespace MythicEmpire.InGame
{
    public class FireTower : AttackTower
    {        
        public float attackAngle = 15f;

        public void Start()
        {
            bullet.Init(Target, damage, exploreRange, bulletSpeed);
            animation.PlayAnimation("stop-fire");

        }
        
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
                    bullet.Explore();
                    FireFlame();
                    StartCoroutine(LoadBullet());
                    CheckTargetRange();

                }

            } 
        }

        private void FireFlame()
        {
            Collider[] results = new Collider[20];
            var numColliders = Physics.OverlapSphereNonAlloc(transform.position, fireRange, results,LayerMask.GetMask("Monster"));

            for (int i = 0; i < numColliders; i++)
            {
                
                if (results[i].TryGetComponent <Monster>(out var monster))
                {
                    Vector3 direction = Target.transform.position - transform.position;
                    direction.y = 0;

                    Vector3 monsterArea = monster.transform.position - transform.position;
                    monsterArea.y = 0;
                    if (Mathf.Abs(Vector3.Angle(direction, monsterArea)) <= attackAngle)
                    {
                        monster.TakeDamage(damage);
                    }
                }
            }
        }
        
    }
}