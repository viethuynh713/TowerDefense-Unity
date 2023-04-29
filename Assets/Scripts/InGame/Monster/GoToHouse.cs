using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MythicEmpire.BehaviorTree;

namespace MythicEmpire.InGame
{
    public class GoToHouse : Node
    {
        private Transform transform;

        public GoToHouse(Transform transform)
        {
            this.transform = transform;
        }

        public override NodeState Evaluate()
        {
            transform.gameObject.GetComponent<Monster>().MoveToHouse();

            state = NodeState.RUNNING;
            return state;
        }
    }
}
