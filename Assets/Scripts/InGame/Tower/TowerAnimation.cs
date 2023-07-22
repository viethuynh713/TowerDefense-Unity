using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace MythicEmpire.InGame
{
    public class TowerAnimation : MonoBehaviour
    {
        public ParticleSystem fireVfx;

        public Animator animator;
        public void PlayAnimation(string state)
        {
            switch (state)
            {
                case "fire":
                    if(fireVfx != null)fireVfx.Play();
                    if(animator!= null)animator.SetTrigger("Fire");
                    break;
            }
        }
    }
}
