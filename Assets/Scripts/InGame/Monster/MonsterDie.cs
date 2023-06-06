using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MythicEmpire.BehaviorTree;

namespace MythicEmpire.InGame
{
    public class MonsterDie : Node
    {
        private Transform transform;
        private bool isAnimPlayed;

        public MonsterDie(Transform transform) {
            this.transform = transform;
            isAnimPlayed = false;
        }

        public override NodeState Evaluate()
        {
            if (!isAnimPlayed)
            {
                transform.gameObject.GetComponent<Animator>().Play("Die01_SwordAndShield");
                isAnimPlayed = true;
            }

            state = NodeState.RUNNING;
            return state;
        }
    }
}
