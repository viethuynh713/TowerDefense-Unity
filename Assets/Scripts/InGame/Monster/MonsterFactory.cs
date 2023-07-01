using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MythicEmpire.InGame
{
    public class MonsterFactory : MonoBehaviour
    {
        [SerializeField] private List<GameObject> monsterList;

        public GameObject GetMonster(string id)
        {
            foreach (GameObject monster in monsterList)
            {
                if (monster.GetComponent<Monster>().Id == id)
                {
                    return monster;
                }
            }
            return null;
        }
    }
}
