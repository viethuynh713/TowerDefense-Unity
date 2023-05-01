using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using MythicEmpire.CommonScript;

namespace MythicEmpire.InGame
{
    public class PlayerController : MonoBehaviour
    {
        private string playerID;
        private int hp;
        private List<GameObject> cardList;
        private bool isMyPlayer;
        private Vector2Int monsterGatePos;

        [SerializeField] private GameObject tower;
        [SerializeField] private GameObject monster;
        // Start is called before the first frame update
        void Start()
        {
            playerID = Random.Range(10000000, 99999999).ToString(); // temp id for testing
            hp = InGameService.playerHp;
            // generate monster (test)
            GenerateMonster();
            BuildTower(5, 5);
        }

        public void Init(bool isMyPlayer)
        {
            this.isMyPlayer = isMyPlayer;
            monsterGatePos = InGameService.monsterGateLogicPos;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void BuildTower(int i, int j)
        {
            GameObject t = Instantiate(tower, InGameService.Logic2DisplayPos(new Vector2Int(i, j), isMyPlayer) + new Vector3(0, 0.16f, 0), Quaternion.identity);
            t.GetComponent<Tower>().Init(isMyPlayer);
        }

        public void GenerateMonster()
        {
            GameObject monsterObj = Instantiate(monster, InGameService.Logic2DisplayPos(monsterGatePos), Quaternion.identity);
            monsterObj.GetComponent<Monster>().Init(playerID, !isMyPlayer);
        }

        public void TakeDmg(int dmg)
        {
            hp -= dmg;
            if (hp <= 0)
            {
                Common.Log("End Game!");
            }
        }

        public string PlayerId { get { return playerID; } }
        public int Hp { get { return hp; } set { hp = value; } }
        public bool IsMyPlayer { get { return isMyPlayer; } }
    }
}