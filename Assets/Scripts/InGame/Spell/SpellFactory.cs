using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MythicEmpire.InGame
{
    public class SpellFactory : MonoBehaviour
    {
        [SerializeField] private List<GameObject> spellList;

        public GameObject GetSpell(string id)
        {
            foreach (GameObject spell in spellList)
            {
                if (spell.GetComponent<Spell>().Id == id)
                {
                    return spell;
                }
            }
            return null;
        }
    }
}
