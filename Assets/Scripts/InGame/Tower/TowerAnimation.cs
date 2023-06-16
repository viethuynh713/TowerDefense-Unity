using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace MythicEmpire.InGame
{
    public class TowerAnimation : MonoBehaviour
    {
        public void PlayAnimation(string id, string state, Transform target = null)
        {
            if (state == "idle")
            {
                switch (id)
                {
                    case "4":
                        GetComponent<GatlingGun>().go_target = null;
                        break;
                }
            }
            else
            {
                switch (id)
                {
                    case "4":
                        GetComponent<GatlingGun>().go_target = target;
                        break;
                }
            }
        }
    }
}
