using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MythicEmpire.InGame
{
    public class MonsterAnimation : MonoBehaviour
    {
        private Animator anim;

        [SerializeField] private string moveAnim;
        [SerializeField] private string attackAnim;
        [SerializeField] private string dieAnim;
        private void Start()
        {
            anim = GetComponent<Animator>();
        }
        public void PlayAnimation(string state)
        {
            switch (state)
            {
                case "move":
                    anim.Play(moveAnim);
                    break;
                case "attack":
                    anim.Play(attackAnim);
                    break;
                case "die":
                    anim.Play(dieAnim);
                    break;
            }
        }
    }
}
