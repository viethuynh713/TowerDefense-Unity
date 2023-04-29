using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MythicEmpire.BehaviorTree;

namespace MythicEmpire.InGame
{
    public class GoToMonster : Node
    {
        private Transform transform;

        public GoToMonster(Transform transform)
        {
            this.transform = transform;
        }

        public override NodeState Evaluate()
        {
            GameObject target = (GameObject)GetData("target");
            if (target != null)
            {
                transform.gameObject.GetComponent<Monster>().MoveToMonster(target.transform);
            }

            state = NodeState.RUNNING;
            return state;
        }
    }
}
