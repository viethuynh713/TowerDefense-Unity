using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MythicEmpire.InGame
{
    public class Healing : Spell
    {
        protected override void Affect()
        {
            // increase monster's hp
            Monster[] monsterList = FindObjectsOfType<Monster>();
            foreach (Monster monster in monsterList)
            {
                if (isMyPlayer == monster.IsMyPlayer)
                {
                    if ((monster.transform.position - transform.position).magnitude < stats.Range)
                    {
                        monster.Heal(Mathf.RoundToInt(stats.DamegePerDuration));
                    }
                }
            }
        }
    }
}
