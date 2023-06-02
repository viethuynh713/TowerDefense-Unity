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
        private string id;
        private string towerId;
        private string ownerId;
        protected TowerStats stats;
        private Vector2Int logicPos;
        protected bool isMyPlayer;
        protected int cost;
        protected int damageLevel;
        protected int rangeLevel;
        protected int attackSpeedLevel;

        [SerializeField] protected Canvas canvas;
        // Start is called before the first frame update
        void Start()
        {
            damageLevel = 1;
            rangeLevel = 1;
            attackSpeedLevel = 1;
        }

        public void Init(string id, bool isMyPlayer, Vector2Int logicPos)
        {
            this.id = id;
            this.isMyPlayer = isMyPlayer;
            this.logicPos = logicPos;
            cost = InGameService.cardCost[new Tuple<CardType, string>(CardType.TowerCard, id)];
            canvas.gameObject.GetComponent<TowerUI>().SetElementPosition(transform.position);
            canvas.gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                canvas.gameObject.SetActive(false);
            }
        }

        public void UpgradeDamage()
        {
            canvas.gameObject.SetActive(false);
            if (damageLevel < InGameService.maxTowerLevel)
            {
                stats.Damage = (stats.Damage + 1 >= stats.Damage * 1.1f) ? stats.Damage + 1 : Mathf.RoundToInt(stats.Damage * 1.1f);
            }
        }

        public void UpgradeRange()
        {
            canvas.gameObject.SetActive(false);
            if (rangeLevel < InGameService.maxTowerLevel)
            {
                stats.Range *= 1.1f;
            }
        }

        public void UpgradeAttackSpeed()
        {
            canvas.gameObject.SetActive(false);
            if (attackSpeedLevel < InGameService.maxTowerLevel)
            {
                stats.AttackSpeed *= 1.1f;
            }
        }

        public void Sell()
        {
            canvas.gameObject.SetActive(false);
            GameController.Instance.SellTower(logicPos, isMyPlayer, cost / 2);
            Destroy(gameObject);
        }

        public void OnMouseUp()
        {
            canvas.gameObject.SetActive(!canvas.gameObject.activeInHierarchy);
        }

        public int Cost { get { return cost; } }
    }
}
