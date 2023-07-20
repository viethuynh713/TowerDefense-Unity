using MythicEmpire.Card;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MythicEmpire.InGame
{
    public class TowerFactory: MonoBehaviour
    {
        [SerializeField] private List<GameObject> towerList;

        public GameObject GetTower(string id)
        {
            foreach (GameObject tower in towerList)
            {
                if (tower.GetComponent<Tower>().TowerID == id)
                {
                    return tower;
                }
            }
            return null;
        }
    }
}
