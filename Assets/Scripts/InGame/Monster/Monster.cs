using MythicEmpire.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.Intrinsics;
using UnityEngine;

namespace MythicEmpire.InGame
{
    public class Monster : MonoBehaviour
    {
        private string id;
        private string ownerId;
        private bool isMyPlayer;
        private bool canAction;
        private bool isSummonedByPlayer;
        private MonsterStats stats;
        //private List<Effect> state;
        private List<Vector2Int> path;

        // Start is called before the first frame update
        void Start()
        {
            path = new List<Vector2Int>();
            stats = new MonsterStats();

            stats.MoveSpeed = 1;
            stats.AttackRange = 3;

            isSummonedByPlayer = false;
            FindPath();
        }

        public void Init(string ownerId, bool isMyPlayer)
        {
            this.ownerId = ownerId;
            this.isMyPlayer = isMyPlayer;
        }

        // Update is called once per frame
        void Update()
        {
            Move();
        }

        public void Move()
        {
            if (path.Count > 0)
            {
                transform.Translate((InGameService.Logic2DisplayPos(path[0]) - transform.position).normalized
                    * stats.MoveSpeed * Time.deltaTime
                );
                if (Mathf.Abs((InGameService.Logic2DisplayPos(path[0]) - transform.position).magnitude) < 0.01f)
                {
                    path.RemoveAt(0);
                }
            }
        }

        public void AttackMonster(GameObject target)
        {
            Debug.Log("Attack Monster");
        }

        public void AttackHouse()
        {

        }

        public void TakeDmg(int dmg)
        {

        }

        public void AddEffect(Effect effect)
        {

        }

        public void ExecuteEffect()
        {

        }

        public void Die()
        {

        }

        public void FindPath()
        {
            path = InGameService.FindPath(
                FindObjectOfType<GameController>().Map.GetComponent<MapService>().CurrentMap,
                InGameService.Display2LogicPos(transform.position),
                InGameService.houseLogicPos[isMyPlayer ? TypePlayer.Player : TypePlayer.Opponent]
            );
        }

        public MonsterStats Stats { get { return stats; } }
    }
}
