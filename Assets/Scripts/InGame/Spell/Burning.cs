using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MythicEmpire.InGame
{
    public class Burning : Spell
    {
        protected override void Affect()
        {
            // decrease monster's hp
            Monster[] monsterList = FindObjectsOfType<Monster>();
            foreach (Monster monster in monsterList)
            {
                if (isMyPlayer != monster.IsMyPlayer)
                {
                    if ((monster.transform.position - transform.position).magnitude < stats.Range)
                    {
                        monster.TakeDmg(Mathf.RoundToInt(stats.Value));
                    }
                }
            }
        }
    }
}
