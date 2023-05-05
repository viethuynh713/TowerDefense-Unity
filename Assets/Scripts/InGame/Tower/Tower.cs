using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MythicEmpire.InGame
{
    public class Tower : MonoBehaviour
    {
        private int id;
        private int ownerId;
        private Vector2Int logicPos;
        protected bool isMyPlayer;
        // Start is called before the first frame update
        void Start()
        {

        }

        public void Init(bool isMyPlayer, Vector2Int logicPos)
        {
            this.isMyPlayer = isMyPlayer;
            this.logicPos = logicPos;
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
            GameController.Instance.SellTower(logicPos);
            Destroy(gameObject);
        }
    }
}
