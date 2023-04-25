using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MythicEmpire.InGame
{
    public class PlayerController : MonoBehaviour
    {
        private string playerID;
        private List<GameObject> cardList;
        private bool isMyPlayer;
        private Vector2Int monsterGatePos;

        [SerializeField] private GameObject monster;
        // Start is called before the first frame update
        void Start()
        {
            playerID = Random.Range(10000000, 99999999).ToString();
            // generate monster (test)
            GenerateMonster();
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

        public void GenerateMonster()
        {
            GameObject monsterObj = Instantiate(monster, InGameService.Logic2DisplayPos(monsterGatePos), Quaternion.identity);
            monsterObj.GetComponent<Monster>().Init(playerID, !isMyPlayer);
        }

        public string PlayerId { get { return playerID; } }
        public bool IsMyPlayer { get { return isMyPlayer; } }
    }
}