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

        public void Init(bool isMyPlayer)
        {
            this.isMyPlayer = isMyPlayer;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Sell()
        {

        }
    }
}
