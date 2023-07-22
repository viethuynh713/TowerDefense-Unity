using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MythicEmpire.InGame
{
    public class Explore : Spell
    {
        protected override void Affect()
        {
            // decrease monster's hp
            Monster[] monsterList = FindObjectsOfType<Monster>();
            foreach (Monster monster in monsterList)
            {
                if (ownerId != monster.OwnerId)
                {
                    if ((monster.transform.position - transform.position).magnitude < stats.Range)
                    {
                        monster.TakeDamage(Mathf.RoundToInt(stats.DamegePerDuration));
                    }
                }
            }
        }

    }
}
