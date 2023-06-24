using System.Collections;
using System.Collections.Generic;
using InGame.Map;
using UnityEngine;
using MythicEmpire.Enums;
using UnityEngine.EventSystems;

namespace MythicEmpire.InGame
{
    public class Tile : MonoBehaviour
    {
        public Vector2Int logicPosition;
        public TypeTile typeOfTile;
        public string ownerId;
        [SerializeField] private bool isBarrier;
        private OwnerType owner;
        private Transform tower;

        public void SetInfo(Vector2Int position, TypeTile type, string ownerId)
        {
            this.logicPosition = position;
            this.typeOfTile = type;
            this.ownerId = ownerId;
        }
        // Start is called before the first frame update
        void Start()
        {
            owner = OwnerType.None;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void BuildTower(Transform tower)
        {
            isBarrier = true;
            this.tower = tower;
        }

        public void SellTower()
        {
            isBarrier = false;
            tower = null;
        }

        public bool IsBarrier { get { return isBarrier; } set { isBarrier = value; } }
        public OwnerType Owner { get { return owner; } set { owner = value; } }
    }
}