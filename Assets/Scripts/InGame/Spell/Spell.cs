using MythicEmpire.Card;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MythicEmpire.InGame
{
    public class Spell : MonoBehaviour
    {
        protected string id;
        protected string ownerId;
        protected SpellStats stats;

        public void Init(string spellId, string playerID, SpellStats spellStats)
        {
            id = spellId;
            stats = spellStats;
            
            ownerId = playerID;
            StartCoroutine(AffectDuration());
            StartCoroutine(Destroy());
        }

        protected virtual void Affect()
        {

        }

        protected IEnumerator AffectDuration()
        {
            Affect();
            yield return new WaitForSeconds(stats.Duration);
            if (stats.Duration > 0)
            {
                StartCoroutine(AffectDuration());
            }
        }

        protected IEnumerator Destroy()
        {
            yield return new WaitForSeconds(stats.Time);
            Destroy(gameObject);
        }

        public string Id { get { return id; } }
        public int Cost { get { return stats.Energy; } }
    }
}
