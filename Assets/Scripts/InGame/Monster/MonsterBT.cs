using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MythicEmpire.BehaviorTree;

namespace MythicEmpire.InGame
{
    public class MonsterBT : BehaviorTree.Tree
    {
        protected override Node SetUpTree()
        {
            Node root = new Selector(new List<Node> {
                new Sequence(new List<Node>
                {
                    new CheckAttackMonster(gameObject),
                    new AttackMonster(gameObject)
                })
            });

            return root;
        }
    }
}
