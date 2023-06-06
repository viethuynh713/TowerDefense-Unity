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

        public CheckAttackMonster(Transform transform) {
            this.transform = transform;
            range = transform.gameObject.GetComponent<Monster>().Stats.AttackRange;
        }

        public override NodeState Evaluate()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, range, InGameService.monsterLayerMask);

            if (colliders.Length > 0)
            {
                GameObject target = null;
                float minDistance = range;
                foreach (Collider collider in colliders)
                {
                    float distance = (transform.position - collider.transform.position).magnitude;
                    if (collider.gameObject != transform.gameObject && distance < minDistance)
                    {
                        Monster component = transform.gameObject.GetComponent<Monster>();
                        Monster colliderComponent = collider.gameObject.GetComponent<Monster>();
                        if ((component.IsSummonedByPlayer || colliderComponent.IsSummonedByPlayer)
                            && (component.IsMyPlayer != colliderComponent.IsMyPlayer)
                            && !component.IsDie && !colliderComponent.IsDie)
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
