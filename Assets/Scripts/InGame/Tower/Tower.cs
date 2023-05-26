using MythicEmpire.Enums;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace MythicEmpire.InGame
{
    public class Tower : MonoBehaviour
    {
        private string id;
        private string ownerId;
        protected TowerStats stats;
        private Vector2Int logicPos;
        protected bool isMyPlayer;
        protected int cost;
        // Start is called before the first frame update
        void Start()
        {

        }

        public void Init(string id, bool isMyPlayer, Vector2Int logicPos)
        {
            this.id = id;
            this.isMyPlayer = isMyPlayer;
            this.logicPos = logicPos;
            cost = InGameService.cardCost[new Tuple<TypeCard, string>(TypeCard.TowerCard, id)];
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Sell()
        {

        }

        public void OnMouseDown()
        {
            GameController.Instance.SellTower(logicPos, isMyPlayer, cost / 2);
            Destroy(gameObject);
        }

        public int Cost { get { return cost; } }
    }
}
