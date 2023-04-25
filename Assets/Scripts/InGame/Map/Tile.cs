using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MythicEmpire.Enums;

namespace MythicEmpire.InGame
{
    public class Tile : MonoBehaviour
    {
        private Vector2Int pos;
        [SerializeField] private bool isBarrier;
        private TypeTile type;
        // private Tower tower;
        // Start is called before the first frame update
        void Start()
        {
            type = TypeTile.None;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public bool IsBarrier { get { return isBarrier; } set { isBarrier = value; } }
        public TypeTile Type { get { return type; } set { type = value; } }
    }
}