using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MythicEmpire.BehaviorTree;

namespace MythicEmpire.InGame
{
    public class CheckAttackMonster : Node
    {
        private Transform transform;
        private float range;

        public CheckAttackMonster(GameObject gameObject) {
            transform = gameObject.transform;
            range = gameObject.GetComponent<Monster>().Stats.AttackRange;
        }

        public override NodeState Evaluate()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, range);

            Debug.Log(colliders.Length);
            if (colliders.Length > 0)
            {
                GameObject target = null;
                float minDistance = Mathf.Infinity;
                foreach (Collider collider in colliders)
                {
                    if (collider.gameObject.GetComponent<Monster>() != null) {
                        float distance = (transform.position - collider.transform.position).magnitude;
                        if (distance < range)
                        {
                            if (distance < minDistance)
                            {
                                target = collider.gameObject;
                                minDistance = distance;
                            }
                        }
                    }
                }
                parent.parent.SetData("target", target);

                state = NodeState.SUCCESS;
                return state;
            }

            state = NodeState.FAILURE;
            return state;
        }
    }
}
