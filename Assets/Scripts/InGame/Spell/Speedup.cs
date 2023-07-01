using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MythicEmpire.InGame
{
    public class Speedup : Spell
    {
        protected override void Affect()
        {
            // decrease monster's hp
            Monster[] monsterList = FindObjectsOfType<Monster>();
            foreach (Monster monster in monsterList)
            {
                if (isMyPlayer == monster.IsMyPlayer)
                {
                    if ((monster.transform.position - transform.position).magnitude < stats.Range)
                    {
                        monster.Speedup(stats.Value);
                    }
                }
            }
        }
    }
}
