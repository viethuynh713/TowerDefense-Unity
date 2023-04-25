using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MythicEmpire.BehaviorTree;

namespace MythicEmpire.InGame
{
    public class AttackMonster : Node
    {
        private GameObject gameObject;

        public AttackMonster(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }

        public override NodeState Evaluate()
        {
            GameObject target = (GameObject)GetData("target");
            gameObject.GetComponent<Monster>().AttackMonster(target);

            state = NodeState.RUNNING;
            return state;
        }
    }
}