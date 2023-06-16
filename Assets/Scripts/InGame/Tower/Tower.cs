using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using MythicEmpire.Card;
using MythicEmpire.Enums;

namespace MythicEmpire.InGame
{
    public class Tower : MonoBehaviour
    {
        [SerializeField] protected string id;
        protected string towerId;
        protected string ownerId;
        [SerializeField] protected TowerStats stats;
        protected Vector2Int logicPos;
        protected bool isMyPlayer;
        protected int damageLevel;
        protected int rangeLevel;
        protected int attackSpeedLevel;
        protected bool canFire;

        protected int damage;
        protected float attackSpeed;
        protected float fireRange;
        protected float exploreRange;
        protected float bulletSpeed;

        [SerializeField] protected Canvas canvas;
        // Start is called before the first frame update
        protected void Start()
        {
            damageLevel = 1;
            rangeLevel = 1;
            attackSpeedLevel = 1;

            damage = stats.Damage;
            attackSpeed = stats.AttackSpeed;
            fireRange = stats.FireRange;
            exploreRange = stats.ExploreRange;
            bulletSpeed = stats.BulletSpeed;

            canFire = true;
        }

        public void Init(string id, bool isMyPlayer, Vector2Int logicPos)
        {
            this.isMyPlayer = isMyPlayer;
            this.logicPos = logicPos;
            canvas.gameObject.GetComponent<TowerUI>().SetElementPosition(transform.position);
            canvas.gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            Fire();
            if (Input.GetMouseButtonDown(0))
            {
                canvas.gameObject.SetActive(false);
            }
        }

        public virtual void Fire()
        {

        }

        public void UpgradeDamage()
        {
            canvas.gameObject.SetActive(false);
            if (damageLevel < InGameService.maxTowerLevel)
            {
                damage = Mathf.RoundToInt(damage * 1.5f);
            }
        }

        public void UpgradeRange()
        {
            canvas.gameObject.SetActive(false);
            if (rangeLevel < InGameService.maxTowerLevel)
            {
                fireRange *= 1.1f;
            }
        }

        public void UpgradeAttackSpeed()
        {
            canvas.gameObject.SetActive(false);
            if (attackSpeedLevel < InGameService.maxTowerLevel)
            {
                attackSpeed *= 1.1f;
            }
        }

        public void Sell()
        {
            canvas.gameObject.SetActive(false);
            GameController.Instance.SellTower(logicPos, isMyPlayer, stats.Energy / 2);
            Destroy(gameObject);
        }

        protected IEnumerator LoadBullet()
        {
            canFire = false;
            yield return new WaitForSeconds(1 / stats.AttackSpeed);
            canFire = true;
        }

        public void OnMouseUp()
        {
            canvas.gameObject.SetActive(!canvas.gameObject.activeInHierarchy);
        }

        public int Cost { get { return stats.Energy; } }
        public string Id { get { return id; } }
    }
}
