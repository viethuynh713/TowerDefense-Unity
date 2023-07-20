using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace MythicEmpire.InGame
{
    public class TowerAnimation : MonoBehaviour
    {
        public ParticleSystem fireVfx;
        public void PlayAnimation(string id, string state, Transform target = null)
        {
            switch (id)
            {
                case "4":
                    switch (state)
                    {
                        case "idle":
                            GetComponent<GatlingGun>().go_target = null;
                            break;
                        case "attack":
                            GetComponent<GatlingGun>().go_target = target;
                            break;
                    }

                    break;
            }
        }

        public void PlayAnimation(string state)
        {
            switch (state)
            {
                case "fire":
                    fireVfx.Play();
                    break;
            }
        }
    }
}
