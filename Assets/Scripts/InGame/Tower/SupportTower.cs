using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MythicEmpire.InGame
{
    public class SupportTower : Tower
    {
        public override void Fire()
        {
            if (canFire)
            {
                GameController.Instance.GainEnergy(damage, true);
                StartCoroutine(LoadBullet());
            }
        }
    }
}
