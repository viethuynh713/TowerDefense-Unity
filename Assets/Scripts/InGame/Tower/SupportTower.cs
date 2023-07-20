using System.Collections;
using System.Collections.Generic;
using Networking_System.Model.Data.DataReceive;
using UnityEngine;

namespace MythicEmpire.InGame
{
    public class SupportTower : Tower
    {
        private AddEnergyData data = new AddEnergyData();

        protected override void Fire()
        {
            if (canFire)
            {
                data.energy = damage;
                data.ownerId = _ownerId;
                data.towerId = _towerID;
                PlayerController_v2.Instance.GainEnergy(data);
                animation.PlayAnimation("fire");
                StartCoroutine(LoadBullet());
            }
        }
    }
}
