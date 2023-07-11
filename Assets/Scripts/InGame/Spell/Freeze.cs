using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MythicEmpire.InGame
{
    public class Freeze : Spell
    {
        protected override void Affect()
        {
            // inactive monster action
            Monster[] monsterList = FindObjectsOfType<Monster>();
            foreach (Monster monster in monsterList)
            {
                if (ownerId != monster.OwnerId)
                {
                    if ((monster.transform.position - transform.position).magnitude < stats.Range)
                    {
                        monster.Freezed(stats.DamegePerDuration);
                    }
                }
            }
        }
    }
}
