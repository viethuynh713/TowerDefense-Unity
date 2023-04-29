using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MythicEmpire.BehaviorTree;

namespace MythicEmpire.InGame
{
    public class CheckGoToMonster : Node
    {
        private static int layerMask = 1 << 3;
        private Transform transform;
        private float range;

        public CheckGoToMonster(Transform transform)
        {
            this.transform = transform;
            range = transform.gameObject.GetComponent<Monster>().Stats.DetectRange;
        }

        public override NodeState Evaluate()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, range, layerMask);

            if (colliders.Length > 0)
            {
                GameObject target = null;
                float minDistance = range;
                foreach (Collider collider in colliders)
                {
                    float distance = (transform.position - collider.transform.position).magnitude;
                    if (collider.gameObject != transform.gameObject && distance < minDistance)
                    {
                        if (transform.gameObject.GetComponent<Monster>().IsSummonedByPlayer
                            && !collider.gameObject.GetComponent<Monster>().IsDie)
                        {
                            target = collider.gameObject;
                            minDistance = distance;
                        }
                    }
                }
                parent.parent.SetData("target", target);

                if (target != null)
                {
                    state = NodeState.SUCCESS;
                    return state;
                }
            }
            parent.parent.SetData("target", null);

            state = NodeState.FAILURE;
            return state;
        }
    }
}
