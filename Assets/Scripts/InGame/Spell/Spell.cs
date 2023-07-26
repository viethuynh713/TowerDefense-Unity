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
        private float time;
        public GameObject model;
        public void Init(string spellId, string playerID, SpellStats spellStats)
        {
            id = spellId;
            stats = spellStats;
            transform.localScale = Vector3.one*stats.Range;
            time = stats.Time;
            ownerId = playerID;
            model.SetActive(false);
            StartCoroutine(AffectDuration());
        }

        protected virtual void Affect()
        {

        }

        protected IEnumerator AffectDuration()
        {
            while (time > 0)
            {
                Affect();
                yield return new WaitForSeconds(stats.Duration);
                time -= stats.Duration;
            }
            Destroy(gameObject);
        }
        

        public string Id { get { return id; } }
        public int Cost { get { return stats.Energy; } }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            if (stats == null)
            {
                Gizmos.DrawWireSphere(transform.position,1);

            }
            else
            {
                Gizmos.DrawWireSphere(transform.position,stats.Range);
            }

        }

        public void View()
        {
            model.SetActive(true);

        }
    }
}
