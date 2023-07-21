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
        protected string _towerID;
        protected string _ownerId;
        // protected TowerStats stats;
        protected bool canFire;

        protected int damage;
        protected float attackSpeed;
        protected float fireRange;
        protected float exploreRange;
        protected float bulletSpeed;

        [SerializeField] protected TowerUI canvas;
        [SerializeField] protected TowerAnimation animation;
        [SerializeField]private Transform rangeUI;
        
        public void Init(string towerId, string ownerId, TowerStats stats)
        {
            _towerID = towerId;
            this._ownerId = ownerId;
            // this.stats = stats;
            
            damage = stats.Damage;
            attackSpeed = stats.AttackSpeed;
            fireRange = stats.FireRange;
            exploreRange = stats.ExploreRange;
            bulletSpeed = stats.BulletSpeed;

            canFire = true;
            
            canvas.SetElementPosition(_towerID, transform.position);
            canvas.gameObject.SetActive(false);
            rangeUI.localScale = new Vector3(fireRange+1, rangeUI.localScale.y, fireRange+1);
            rangeUI.gameObject.SetActive(false);

        }

        // Update is called once per frame
        void Update()
        {
            Fire();
            if (Input.GetMouseButtonDown(1))
            {
                canvas.gameObject.SetActive(false);
                rangeUI.gameObject.SetActive(false);

            }
        }

        protected virtual void Fire()
        {

        }
        
        public void Sell()
        {
            canvas.gameObject.SetActive(false);
            Destroy(gameObject);
        }

        protected IEnumerator LoadBullet()
        {
            canFire = false;
            yield return new WaitForSeconds(1 / attackSpeed);
            canFire = true;
        }

        public void ActiveUI()
        {
            rangeUI.gameObject.SetActive(true);

            canvas.gameObject.SetActive(!canvas.gameObject.activeInHierarchy);
        }

        public string TowerID => _towerID;
        public string OwnerId => _ownerId;

        public void Upgrade(UpgradeTowerDataSender upgradeTowerDataSender)
        {
            damage = upgradeTowerDataSender.damage;
            attackSpeed = upgradeTowerDataSender.attackSpeed;
            fireRange = upgradeTowerDataSender.range;
            rangeUI.localScale = new Vector3(fireRange+1, rangeUI.localScale.y, fireRange+1);
            canvas.gameObject.SetActive(false);
            rangeUI.gameObject.SetActive(false);


        }
    }
}
