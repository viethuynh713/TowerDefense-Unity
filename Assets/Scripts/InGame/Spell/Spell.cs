using MythicEmpire.Card;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MythicEmpire.InGame
{
    public class Spell : MonoBehaviour
    {
        [SerializeField] protected string id;
        protected string ownerId;
        protected bool isMyPlayer;
        [SerializeField] protected SpellStats stats;
        protected Vector3 pos;

        public void Init(string playerID, bool isMyPlayer)
        {
            ownerId = playerID;
            this.isMyPlayer = isMyPlayer;
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
