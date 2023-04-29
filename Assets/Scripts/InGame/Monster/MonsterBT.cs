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
                    new CheckMonsterDie(transform),
                    new MonsterDie(transform)
                }),
                new Sequence(new List<Node>
                {
                    new CheckAttackMonster(transform),
                    new AttackMonster(transform)
                }),
                new GoToHouse(transform)
            });

            return root;
        }
    }
}
