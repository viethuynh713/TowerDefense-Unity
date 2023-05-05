using MythicEmpire.Enums;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace MythicEmpire.InGame
{
    public class GameController : MonoBehaviour
    {
        private static GameController instance;
        private string gameID;
        private List<GameObject> playerList;
        [SerializeField] private GameObject map;
        private GameState state;

        [SerializeField] private GameObject playerController;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            // initial 2 playerController (the first in playerList is myPlayer)
            playerList = new List<GameObject>();
            for (int i = 0; i < InGameService.nPlayer; i++)
            {
                GameObject player = Instantiate(
                    playerController, InGameService.Logic2DisplayPos(InGameService.mapLogicPos), Quaternion.identity
                );
                player.GetComponent<PlayerController>().Init(i == 0 ? true : false);
                playerList.Add(player);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public static GameController Instance { get { return instance; } }

        public GameObject GetPlayer(string id)
        {
            foreach (GameObject player in playerList)
            {
                if (player.GetComponent<PlayerController>().PlayerId == id)
                {
                    return player;
                }
            }
            return null;
        }

        public GameObject GetPlayer(bool isMyPlayer)
        {
            foreach (GameObject player in playerList)
            {
                if (player.GetComponent<PlayerController>().IsMyPlayer == isMyPlayer)
                {
                    return player;
                }
            }
            return null;
        }

        public void BuildTower(Vector3 displayPos, bool isMyPlayer)
        {
            int index = isMyPlayer ? 0 : 1;
            playerList[index].GetComponent<PlayerController>().BuildTower(displayPos);
        }

        public void SellTower(Vector2Int logicPos)
        {
            map.GetComponent<MapService>().SellTower(logicPos);
        }

        public GameObject Map { get { return map; } }
    }
}