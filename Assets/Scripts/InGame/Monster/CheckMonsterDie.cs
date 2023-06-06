using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MythicEmpire.BehaviorTree;

namespace MythicEmpire.InGame
{
    public class CheckMonsterDie : Node
    {
        private Transform transform;

        public CheckMonsterDie(Transform transform) {
            this.transform = transform;
        }

        public override NodeState Evaluate()
        {
            if (transform.gameObject.GetComponent<Monster>().IsDie)
            {
                state = NodeState.SUCCESS;
                return state;
            }

            state = NodeState.FAILURE;
            return state;
        }
    }
}
