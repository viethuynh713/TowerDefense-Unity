using System.Collections;
using System.Collections.Generic;
using Networking_System.Model.Data.DataReceive;
using UnityEngine;

namespace MythicEmpire.InGame
{
    public class SupportTower : Tower
    {
        private AddEnergyData data = new AddEnergyData();
        
        public override void Fire()
        {
            if (canFire)
            {
                data.energy = damage;
                data.ownerId = ownerId;
                data.towerId = id;
                PlayerController_v2.Instance.GainEnergy(data);
                StartCoroutine(LoadBullet());
            }
        }
    }
}
