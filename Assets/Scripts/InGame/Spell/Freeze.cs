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
                        monster.Speedup(-stats.DamegePerDuration);
                    }
                }
            }
            Tower[] towers = FindObjectsOfType<Tower>();
            foreach (Tower tower in towers)
            {
                if (ownerId != tower.OwnerId)
                {
                    if ((tower.transform.position - transform.position).magnitude < stats.Range)
                    {
                        tower.Speedup(-stats.DamegePerDuration);
                    }
                }
            }
        }
    }
}
