using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using MythicEmpire.Card;
using MythicEmpire.Enums;
using MythicEmpire.Manager.MythicEmpire.Manager;

namespace MythicEmpire.InGame
{
    public class Tower : MonoBehaviour
    {
        protected string id;
        protected string ownerId;
        protected TowerStats stats;
        protected Vector2Int logicPos;
        protected int damageLevel;
        protected int rangeLevel;
        protected int attackSpeedLevel;
        protected bool canFire;

        protected int damage;
        protected float attackSpeed;
        protected float fireRange;
        protected float exploreRange;
        protected float bulletSpeed;

        [SerializeField] protected TowerUI canvas;

        public void Start()
        {
        }

        public void Init(string towerId, string ownerId, Vector2Int logicPos, TowerStats stats)
        {
            id = towerId;
            this.ownerId = ownerId;
            this.logicPos = logicPos;
            this.stats = stats;
            
            damageLevel = 1;
            rangeLevel = 1;
            attackSpeedLevel = 1;

            damage = stats.Damage;
            attackSpeed = stats.AttackSpeed;
            fireRange = stats.FireRange;
            exploreRange = stats.ExploreRange;
            bulletSpeed = stats.BulletSpeed;

            canFire = true;
            
            canvas.SetElementPosition(id, transform.position);
            canvas.gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            Fire();
            if (Input.GetMouseButtonDown(2))
            {
                canvas.gameObject.SetActive(false);
            }
        }

        public virtual void Fire()
        {

        }

        // public void UpgradeDamage()
        // {
        //     canvas.gameObject.SetActive(false);
        //     // if (damageLevel < InGameService.maxTowerLevel)
        //     // {
        //     //     damage = Mathf.RoundToInt(damage * 1.5f);
        //     // }
        // }
        //
        // public void UpgradeRange()
        // {
        //     canvas.gameObject.SetActive(false);
        //     // if (rangeLevel < InGameService.maxTowerLevel)
        //     // {
        //     //     fireRange *= 1.1f;
        //     // }
        // }
        //
        // public void UpgradeAttackSpeed()
        // {
        //     canvas.gameObject.SetActive(false);
        //     // if (attackSpeedLevel < InGameService.maxTowerLevel)
        //     // {
        //     //     attackSpeed *= 1.1f;
        //     // }
        // }

        public void Sell()
        {
            canvas.gameObject.SetActive(false);
            Destroy(gameObject);
        }

        protected IEnumerator LoadBullet()
        {
            canFire = false;
            yield return new WaitForSeconds(1 / stats.AttackSpeed);
            canFire = true;
        }

        public void ActiveUI()
        {
            canvas.gameObject.SetActive(!canvas.gameObject.activeInHierarchy);
        }

        public int Cost { get { return stats.Energy; } }
        public string Id { get { return id; } }
        public string OwnerId { get { return ownerId; } }

        public void Upgrade(UpgradeTowerDataSender upgradeTowerDataSender)
        {
            damage = upgradeTowerDataSender.damage;
            attackSpeed = upgradeTowerDataSender.attackSpeed;
            fireRange = upgradeTowerDataSender.range;
            canvas.gameObject.SetActive(false);


        }
    }
}
