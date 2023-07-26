using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MythicEmpire.InGame
{
    public class ConeBullet : Bullet
    {
        public float attackAngle = 15f;
        public override void Move()
        {

        }

        public override void Explore()
        {
            // Collider[] results = new Collider[20];
            // var numColliders = Physics.OverlapSphereNonAlloc(transform.position, f, results,LayerMask.GetMask("Monster"));
            //
            // for (int i = 0; i < numColliders; i++)
            // {
            //     
            //     if (results[i].TryGetComponent <Monster>(out var monster))
            //     {
            //         Vector3 direction = target.transform.position - transform.position;
            //         direction.y = 0;
            //
            //         Vector3 monsterArea = monster.transform.position - transform.position;
            //         monsterArea.y = 0;
            //         if (Mathf.Abs(Vector3.Angle(direction, monsterArea)) <= attackAngle)
            //         {
            //             monster.TakeDamage(damage);
            //         }
            //     }
            // }
            // Monster[] monsterList = FindObjectsOfType<Monster>();
            // foreach (Monster monster in monsterList)
            // {
            //     // check if monster is in explore range and in degree of explore
            //     if ((monster.transform.position - transform.position).magnitude < exploreRange)
            //     {
            //         Vector3 direction = target.transform.position - transform.position;
            //         direction.y = 0;
            //         
            //         Vector3 monsterArea = 
            //         if (Vector3.Angle(monster.transform.position - transform.position, target.transform.position - transform.position) <= attackAngle)
            //         {
            //             monster.TakeDamage(damage);
            //         }
            //     }
            // }
            // Destroy(gameObject);
        }
        
                
    }
}
